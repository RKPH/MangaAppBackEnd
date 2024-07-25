using System.Security.Claims;
using MangaApp.Data;
using MangaApp.DTO;
using MangaApp.Interfaces;
using MangaApp.Model.Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace MangaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserMangaController(IUserMangaRepository userMangaRepository) : ControllerBase
    {
        [HttpPost("savemanga")]
        public async Task<ActionResult> SaveManga(Guid userId, [FromBody] mangaDto? mangaDto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || userIdClaim.Value != userId.ToString())
            {
                return Unauthorized("Invalid token.");
            }
            
            if (mangaDto == null)
            {
                return BadRequest("Manga data is required.");
            }

            // Check if the manga is already saved by the user
            var existingManga = await userMangaRepository.FindUserMangaAsync(userId, mangaDto.Slug); 

            if (existingManga != null)
            {
                return Conflict("This manga is already saved by the user.");
            }

            var manga = new UserManga
            {
                UserId = userId,
                Slug = mangaDto.Slug,
                MangaName = mangaDto.MangaName,
                MangaImage = mangaDto.MangaImage,
                SaveType = mangaDto.SaveType 
            };

            try
            {
                await userMangaRepository.AddUserMangaAsync(manga);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error saving manga data.");
            }

            return Ok();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("deletemanga")]
        public async Task<IActionResult> DeleteManga(Guid userId,[FromBody] string slug)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Invalid token.");
            }

            // Check if the manga exists and belongs to the user
            var userManga = await userMangaRepository.FindUserMangaAsync(userId, slug);

            if (userManga == null)
            {
                return NotFound("Manga not found.");
            }

            try
            {
                await userMangaRepository.DeleteUserMangaAsync(userManga);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting manga data.");
            }

            return NoContent();
        }
    }
}
