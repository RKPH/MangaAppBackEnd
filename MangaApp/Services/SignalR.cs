using MangaApp.DTO;
using Microsoft.AspNetCore.SignalR;

namespace MangaApp.Services;

public class SignalR:Hub 
{
    public async Task SendComment(CommentDto comment)
    {
        await Clients.All.SendAsync("ReceiveComment", comment);
    }

    public async Task SendLike(LikeCommentRequestDto like)
    {
        await Clients.All.SendAsync("ReceiveLike", like);
    }
}