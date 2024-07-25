namespace MangaApp.DTO;

public class LikeCommentRequestDto
{
   public Guid CommentId { get; set; }
   public Guid UserId { get; set; }
}