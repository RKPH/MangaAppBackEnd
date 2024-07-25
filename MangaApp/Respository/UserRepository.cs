using MangaApp.Data;
using MangaApp.DTO;
using MangaApp.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MangaApp.Respository;

public class UserRepository : IUserRepository
{
    private readonly MangaAppDbcontext _context;

    public UserRepository(MangaAppDbcontext context)
    {
        _context = context;
    }

    public async Task<UserDto> GetUserByIdAsync(Guid userId)
    {
        var user = await _context.Users
            .Include(u => u.UserMangas)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (user == null)
        {
            return null;
        }

        var userDto = new UserDto
        {
            UserID = user.UserId.ToString(),
            UserEmail = user.UserEmail,
            UserName = user.UserName,
            Avatar = user.Avatar,
            CreatedAt = user.CreatedAt,
            IsBanned = user.IsBanned,
            FaceAuthenticationImage = " ",
            UserMangas = user.UserMangas.Select(um => new UserMangaDto
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


    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users =await _context.Users.Include(u => u.UserMangas)
            .Select(u => new UserDto
            {
                UserID = u.UserId.ToString(),
                UserEmail = u.UserEmail,
                UserName = u.UserName,
                Avatar = u.Avatar,
                CreatedAt = u.CreatedAt,
                IsBanned = u.IsBanned,

                UserMangas = u.UserMangas.Select(um => new UserMangaDto
                {
                    MangaId = um.MangaId,
                    UserId = um.UserId,
                    Slug = um.Slug,
                    MangaName = um.MangaName,
                    MangaImage = um.MangaImage,
                }).ToList()
            }).ToListAsync();
        return users;
    }

    public async Task<UserDto> UpdateUserAsync(Guid userId, EditingUserDto? userDto)
    {
        var user = await _context.Users.Include(user => user.UserMangas).FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
        {
            return null; // Indicate that the user was not found
        }

        user.UserName = userDto?.UserName;
        user.Avatar = userDto?.Avatar;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        // Return the updated user as UserDto
        var updatedUserDto = new UserDto
        {
            UserID = user.UserId.ToString(),
            UserEmail = user.UserEmail,
            UserName = user.UserName,
            Avatar = user.Avatar,
            CreatedAt = user.CreatedAt,
            IsBanned = user.IsBanned,
            UserMangas = user.UserMangas.Select(um => new UserMangaDto
            {
                MangaId = um.MangaId,
                UserId = um.UserId,
                Slug = um.Slug,
                MangaName = um.MangaName,
                MangaImage = um.MangaImage
            }).ToList()
        };

        return updatedUserDto;
    }

    public async Task UpdatePointAsync(Guid userId, RequestUpdatePointDto requestUpdatePointDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
        {
            throw new KeyNotFoundException("User not found");
        }

        if (user.Point < requestUpdatePointDto.PointNeeded)
        {
            throw new InvalidOperationException("Not enough points");
        }

        user.Point -= requestUpdatePointDto.PointNeeded;
        user.Point += requestUpdatePointDto.Point;

        await _context.SaveChangesAsync();
    }

   

}