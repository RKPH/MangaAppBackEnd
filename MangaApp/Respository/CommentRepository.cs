using MangaApp.Data;
using MangaApp.DTO;
using MangaApp.Interfaces;
using MangaApp.Model.Domain;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace MangaApp.Respository;

public class CommentRepository(MangaAppDbcontext dbContext) : ICommentRepository
{
    public async Task<Guid> AddCommentAsync(CommentDto commentDto)
    {
        var user = await dbContext.Users.FindAsync(commentDto.UserID);
        

        var comment = new Comments
        {
            CommentId = Guid.NewGuid(),
            UserId = commentDto.UserID,
            slug = commentDto.Slug,
            Comment = commentDto.Comment,
            CreatedAt = DateTime.UtcNow,
        };

        dbContext.Comments.Add(comment);
        await dbContext.SaveChangesAsync();
        return comment.CommentId;
    }
    

    public async Task<List<CommentReponseDto>> GetCommentsBySlugAsync(string slug)
    {
        return await dbContext.Comments
            .Where(c => c.slug == slug)
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new CommentReponseDto
            {
                CommentId = c.CommentId,
                Comment = c.Comment,
                CreatedAt = c.CreatedAt,
                Like = c.Like,
                User = new UserDto
                {
                    UserID = c.User.UserId.ToString(),
                    UserName = c.User.UserName,
                    Avatar = c.User.Avatar,
                }
            }).ToListAsync();
    }
    public async Task LikeCommentAsync(LikeCommentRequestDto likeCommentRequestDto)
    {
        var comment = await dbContext.Comments.FindAsync(likeCommentRequestDto.CommentId);
        var isLiked = await dbContext.CommentLikes
            .AnyAsync(cl => cl.CommentId == likeCommentRequestDto.CommentId && cl.UserId == likeCommentRequestDto.UserId);
        if (isLiked)
        {
            throw new HttpRequestException("You have already liked this comment.");
        }
        var CommentLike = new CommentLikes
        {
            
            CommentId = likeCommentRequestDto.CommentId,
            UserId = likeCommentRequestDto.UserId,
           
        };
        comment.Like += 1;
        dbContext.CommentLikes.Add(CommentLike);
        await dbContext.SaveChangesAsync();
    }
    public async Task<CommentLikes> GetCommentLikeAsync(Guid commentId, Guid userId)
    {
        return await dbContext.CommentLikes
            .FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.UserId == userId);
    }
    public async Task<User> GetUserByIdAsync(Guid userId)
    {
        return await dbContext.Users.FindAsync(userId);
    }
}