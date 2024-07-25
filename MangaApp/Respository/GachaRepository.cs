using MangaApp.Data;
using MangaApp.DTO;
using MangaApp.Interfaces;
using MangaApp.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace MangaApp.Respository;

public class GachaRepository : IGachaRepository
{
    private readonly MangaAppDbcontext dbContext;

    public GachaRepository(MangaAppDbcontext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AddGachaAsync(GachaDto gachaDto)
    {
        try
        {
            var newGacha = new Gacha
            {
                Id = Guid.NewGuid(),
                name = gachaDto.name,
                thumb_url = gachaDto.thumb_url,
                rarity = gachaDto.rarity,
                bgColor = gachaDto.bgColor,
                rate = gachaDto.rate,
                point = gachaDto.point
            };

            dbContext.GachaItems.Add(newGacha);
            await dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Handle or log the exception as needed
            throw new Exception("An error occurred while adding the gacha item.", ex);
        }
    }


    public async Task<List<GachaDto>> GetGachaAsync()
    {
        try
        {
            var gachaItems = await dbContext.GachaItems.ToListAsync();

            var gachaDtoList = gachaItems.Select(g => new GachaDto
            {
                Id = g.Id,
                name = g.name,
                thumb_url = g.thumb_url,
                rarity = g.rarity,
                bgColor = g.bgColor,
                rate = g.rate,
                point = g.point
            }).ToList();

            return gachaDtoList;
        }
        catch (Exception ex)
        {
            // Handle or log the exception as needed
            throw new Exception("An error occurred while retrieving the gacha items.", ex);
        }
    }

}