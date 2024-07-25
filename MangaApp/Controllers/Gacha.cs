using MangaApp.DTO;
using MangaApp.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MangaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Gacha : ControllerBase
    {
        private readonly IGachaRepository _gachaRepository;
        public Gacha(IGachaRepository gachaRepository)
        {
            _gachaRepository = gachaRepository;
        }
        [HttpPost ("addgacha")]
        public async Task<ActionResult> AddGacha([FromBody] GachaDto? gachaDto)
        {
            if (gachaDto == null)
            {
                return BadRequest("Gacha data is required.");
            }
            try
            {
                await _gachaRepository.AddGachaAsync(gachaDto);
            }
            catch (Exception ex)
            {
                // Log the exception (not implemented here)
                return StatusCode(StatusCodes.Status500InternalServerError, "Error adding gacha data.");
            }
            return Ok();
        }
        [HttpGet("getgachaItems")]
        public async Task<ActionResult> GetGachaItems()
        {
            var gachaItems = await _gachaRepository.GetGachaAsync();
            return Ok(gachaItems);
        }
    }
}
