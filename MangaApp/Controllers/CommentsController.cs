
using MangaApp.DTO;
using Microsoft.AspNetCore.Mvc;

using MangaApp.Interfaces;
using MangaApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MangaApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IHubContext<SignalR> _hubContext;
        public CommentsController(ICommentRepository commentRepository, IHubContext<SignalR> hubContext)
        {
            _commentRepository = commentRepository;
            _hubContext = hubContext;
        }
 
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("addcomment/{slug}")]
        public async Task<IActionResult> CreateComment([FromBody] CommentDto commentDto)
        {
            if (string.IsNullOrWhiteSpace(commentDto.Slug))
            {
                return BadRequest("Slug must be included.");
            }

            if (commentDto == null || string.IsNullOrWhiteSpace(commentDto.Comment) ||
                commentDto.UserID == Guid.Empty)
            {
                return BadRequest("Comment content and UserID must be included.");
            }


            var user = await _commentRepository.GetUserByIdAsync(commentDto.UserID);
            if (user == null)
            {
                return NotFound("User not found.");
            }

           

            await _commentRepository.AddCommentAsync(commentDto);
            await _hubContext.Clients.All.SendAsync("ReceiveComment", commentDto);
            return Ok(new { message = "Comment added successfully !!!." });
        }

        [HttpGet("comments/{slug}")]
        public async Task<IActionResult> GetComments(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return BadRequest("Slug is required.");
            }

            var comments = await _commentRepository.GetCommentsBySlugAsync(slug);

            if (comments == null || comments.Count == 0)
            {
                return NoContent();
            }
            

            return Ok(comments);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("likecomment")]
        public async Task<IActionResult> LikeComment([FromBody] LikeCommentRequestDto likeCommentRequestDto)
        {
            if (likeCommentRequestDto == null || likeCommentRequestDto.CommentId == Guid.Empty ||
                likeCommentRequestDto.UserId == Guid.Empty)
            {
                return BadRequest("CommentId and UserId are required.");
            }
            
            var user = await _commentRepository.GetUserByIdAsync(likeCommentRequestDto.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var commentLiked = await _commentRepository.GetCommentLikeAsync(likeCommentRequestDto.CommentId, likeCommentRequestDto.UserId);
            if (commentLiked != null)
            {
                return BadRequest("You have already liked this comment.");
            }

            await _commentRepository.LikeCommentAsync(likeCommentRequestDto);
            await _hubContext.Clients.All.SendAsync("ReceiveLike", likeCommentRequestDto);
            return Ok(new { message = "Comment liked successfully !!!." });
        }
    }
}