using MangaApp.Data;
using MangaApp.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using MangaApp.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MangaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(MangaAppDbcontext dbContext, IUserRepository userRepository)
        : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetAllUser()
        {
            var users = await userRepository.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(Guid id)
        {
            var user = await userRepository.GetUserByIdAsync(id);

            return Ok(user);
        }

        [HttpPut("face-authentication/{id}")]
        public async Task<IActionResult> UpdateFaceAuthenticationImage(Guid id, [FromBody] UpdateFaceDto updateDto)
        {
            var user = await userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            user.FaceAuthenticationImage = updateDto.FaceAuthenticationImage;

            try
            {
                await dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the database.");
            }
        }

        [HttpPut("/Update/{id}")]
        public async Task<IActionResult> UpdateAvatar(Guid id, [FromBody] EditingUserDto? updateDto)
        {
            if (updateDto == null)
            {
                return BadRequest("User data is null.");
            }

            var updatedUserDto = await userRepository.UpdateUserAsync(id, updateDto);

            if (updatedUserDto == null)
            {
                return NotFound();
            }

            return Ok(updatedUserDto); // Return 200 OK with the updated user data
        }
        [HttpPost("updatePoint/{id}")]
        public async Task<IActionResult> UpdatePoint(Guid id, [FromBody] RequestUpdatePointDto requestUpdatePointDto)
        {
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.Point < requestUpdatePointDto.PointNeeded)
            {
                return BadRequest("Not enough points");
            }

            user.Point -= requestUpdatePointDto.PointNeeded;
            user.Point += requestUpdatePointDto.Point;
            try
            {
                await dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating the database.");
            }
        }

        
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Unauthorized("You are not authorized to delete a user.");
            }
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            dbContext.Users.Remove(user);

            try
            {
                await dbContext.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting the user.");
            }
        }
    }
}