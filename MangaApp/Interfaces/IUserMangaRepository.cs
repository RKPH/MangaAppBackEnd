using MangaApp.Model.Domain;

namespace MangaApp.Interfaces;

public interface IUserMangaRepository
{
    Task<UserManga?> FindUserMangaAsync(Guid userId, string slug);
    Task AddUserMangaAsync(UserManga userManga);
    Task DeleteUserMangaAsync(UserManga userManga);
}