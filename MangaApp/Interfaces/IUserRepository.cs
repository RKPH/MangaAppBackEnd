using MangaApp.DTO;
using Microsoft.AspNetCore.Mvc;


namespace MangaApp.Interfaces;

public interface IUserRepository
{
   Task<UserDto> GetUserByIdAsync(Guid userId);
   Task<List<UserDto>> GetAllUsersAsync();
   Task<UserDto> UpdateUserAsync(Guid userId, EditingUserDto? user);
   Task UpdatePointAsync(Guid userId, RequestUpdatePointDto requestUpdatePointDto);
}