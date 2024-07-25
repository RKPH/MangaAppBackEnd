using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaApp.Model.Domain;

public class Comments
{
    [Key]
    public Guid CommentId { get; set; }    
   
    public Guid UserId { get; set; }
    public string slug { get; set; }
    public string Comment { get; set; }
    public int Like { get; set; }
    public DateTime CreatedAt { get; set; }
    [ForeignKey("UserId")]
    public virtual User User { get; set; }
    public virtual ICollection<CommentLikes> CommentLikes { get; set; } = new List<CommentLikes>();
    
    
}