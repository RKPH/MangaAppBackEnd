namespace MangaApp.DTO;

public class UserDto
{
    public string UserID { get; set; }
    public string? UserEmail {get; set;}
    public string? UserName { get; set; }
    public string? Avatar { get; set; }
    public long Point { get; set; }
    public string FaceAuthenticationImage { get; set; }
    public bool IsBanned { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<UserMangaDto> ? UserMangas { get; set; } // Add this line
}