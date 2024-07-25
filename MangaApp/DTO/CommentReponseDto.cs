namespace MangaApp.DTO;

public class CommentReponseDto
{
    public Guid CommentId { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Like { get; set; }
    public UserDto User { get; set; }
}