using MangaApp.DTO;
using MangaApp.Model.Domain;
using Gacha = MangaApp.Controllers.Gacha;

namespace MangaApp.Interfaces;

public interface IGachaRepository
{
    Task AddGachaAsync(GachaDto gachaDto);
    Task<List<GachaDto>> GetGachaAsync();
}