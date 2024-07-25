using MangaApp.DTO;
using MangaApp.Model.Domain;

namespace MangaApp.Interfaces;

public interface ICommentRepository
{
 
    Task<List<CommentReponseDto>> GetCommentsBySlugAsync(string slug); 
    Task AddCommentAsync(CommentDto comment);
    Task LikeCommentAsync(LikeCommentRequestDto likeCommentRequestDto);
    Task<User> GetUserByIdAsync(Guid userId);
    Task<CommentLikes> GetCommentLikeAsync(Guid commentId, Guid userId);
}