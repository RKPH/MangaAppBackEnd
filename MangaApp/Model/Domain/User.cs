using MySqlConnector;
using System;
using System.Collections.Generic;

namespace MangaApp.Model.Domain
{
    public class User
    {
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        public string? Avatar { get; set; }
        public string? FaceAuthenticationImage { get; set; }
        public long Point { get; set; }
        public byte[]? HashPassword { get; set; }
        public byte[]? SaltPassword { get; set; }
        public bool IsBanned { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public string Role { get; set; }
        // Initializing the collection in the constructor to avoid null reference exceptions
        public virtual ICollection<UserManga> UserMangas { get; set; } = new List<UserManga>();
        public virtual ICollection<Comments> _Comments { get; set; } = new List<Comments>();
        public virtual ICollection<CommentLikes> CommentLikes { get; set; } = new List<CommentLikes>();
        public User()
        {
            UserId = Guid.NewGuid();
            Avatar = "https://img.freepik.com/free-photo/person-preparing-get-driver-license_23-2150167558.jpg?t=st=1714792408~exp=1714796008~hmac=66a1407366466c5cf9538e70626c0d88501ab59383a7416a53667743666f4247&w=360";
            CreatedAt = DateTime.UtcNow;
            IsBanned = false;
            Role = "User";
            Point = 100;
        }
    }
}