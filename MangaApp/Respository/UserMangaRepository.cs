using MangaApp.Data;
using MangaApp.Interfaces;
using MangaApp.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace MangaApp.Respository;

public class UserMangaRepository(MangaAppDbcontext dbContext) : IUserMangaRepository
{
    public async Task<UserManga?> FindUserMangaAsync(Guid userId, string slug)
    {
        return await dbContext.UserMangas
            .FirstOrDefaultAsync(um => um.UserId == userId && um.Slug == slug);
    }

    public Task AddUserMangaAsync(UserManga userManga)
    {
        dbContext.UserMangas.Add(userManga);
        return dbContext.SaveChangesAsync();
    }

    public Task DeleteUserMangaAsync(UserManga userManga)
    {
        dbContext.UserMangas.Remove(userManga);
        return dbContext.SaveChangesAsync();
    }
}