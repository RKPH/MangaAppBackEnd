using System.ComponentModel.DataAnnotations.Schema;

namespace MangaApp.Model.Domain;

public class CommentLikes
{
    public Guid CommentLikeId { get; set; }
    public Guid CommentId { get; set; }
    public Guid UserId { get; set; }
    [ForeignKey("CommentId")]
    public virtual Comments Comment { get; set; }
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
}