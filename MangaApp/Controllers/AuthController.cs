using System;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Google.Apis.Auth;
using MangaApp.Data;
using MangaApp.DTO;
using MangaApp.Interfaces;
using MangaApp.Model.Domain;

namespace MangaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MangaAppDbcontext _context;
        private readonly ITokenService _tokenService;

        public AuthController(MangaAppDbcontext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserEmail)) return BadRequest("UserEmail is already taken");

            var user = CreateUser(registerDto.UserName, registerDto.UserEmail, registerDto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthDto
            {
                Token = _tokenService.CreateToken(user),
            };
        }

        [HttpPost("register/google")]
        public async Task<ActionResult<AuthDto>> GoogleRegister(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);

            if (await UserExists(payload.Email)) return BadRequest("A user with this email already exists.");

            var user = new User
            {
                UserName = payload.Name,
                UserEmail = payload.Email,
                Avatar = payload.Picture,
                Role = "User",  
                IsBanned = false,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthDto
            {
                Token = _tokenService.CreateToken(user),
            };
        }

        [HttpPost("login/google")]
        public async Task<ActionResult<AuthDto>> GoogleLogin(string idToken)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken);
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserEmail == payload.Email);

            if (user == null) return BadRequest("User not found.");


            return new AuthDto
            {
                Token = _tokenService.CreateToken(user),
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthDto>> Login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserEmail == loginDto.UserEmail);

            if (user == null) return Unauthorized("Invalid UserEmail");

            if (!VerifyPassword(loginDto.Password, user.HashPassword, user.SaltPassword))
                return Unauthorized("Invalid Password");
            Response.Cookies.Append("Token", _tokenService.CreateToken(user), new Microsoft.AspNetCore.Http.CookieOptions
            {
                HttpOnly = true,
                
                SameSite = SameSiteMode.None,
                Secure = true
            });
            return new AuthDto
            {
                Token = _tokenService.CreateToken(user),
                refreshToken = _tokenService.CreateRefreshToken(user) 
            };
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<UserDto>> GetUserProfile()
        {
            var userEmailClaim =
                HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userEmailClaim)) return BadRequest("User ID claim not found.");

            var user = await _context.Users
                .Include(u => u.UserMangas)
                .FirstOrDefaultAsync(u => u.UserId.ToString() == userEmailClaim);

            if (user == null) return NotFound("User not found.");

            var userDto = new UserDto
            {
                UserID = user.UserId.ToString(),
                UserEmail = user.UserEmail,
                UserName = user.UserName,
                Avatar = user.Avatar,
                CreatedAt = user.CreatedAt,
                Point = user.Point,
                Role = user.Role,
                IsBanned = user.IsBanned,
                UserMangas = user.UserMangas?.Select(um => new UserMangaDto
                {
                    MangaId = um.MangaId,
                    UserId = um.UserId,
                    Slug = um.Slug,
                    MangaName = um.MangaName,
                    MangaImage = um.MangaImage
                }).ToList()
            };

            return userDto;
        }

        private async Task<bool> UserExists(string userEmail)
        {
            return await _context.Users.AnyAsync(x => x.UserEmail.ToLower() == userEmail.ToLower());
        }

        private User CreateUser(string userName, string userEmail, string password)
        {
            using var hmac = new HMACSHA512();
            return new User
            {
                UserName = userName,
                UserEmail = userEmail,
                HashPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                SaltPassword = hmac.Key,
               
              
            };
        }

        private bool VerifyPassword(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != storedHash[i]) return false;
            }

            return true;
        }
    }
}