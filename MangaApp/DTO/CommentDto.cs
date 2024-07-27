namespace MangaApp.DTO;

public class CommentDto
{
    public Guid  UserID { get; set; }
    public string Comment { get; set; }
    public string Slug { get; set; }
    public string UserName { get; set; }
    public string Avatar { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
}