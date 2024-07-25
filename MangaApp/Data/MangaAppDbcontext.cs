using MangaApp.Model.Domain;
using Microsoft.EntityFrameworkCore;

namespace MangaApp.Data
{
    public class MangaAppDbcontext : DbContext
    {
        public MangaAppDbcontext(DbContextOptions<MangaAppDbcontext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserManga> UserMangas { get; set; }
        public DbSet<Gacha> GachaItems { get; set; }
        public DbSet<Comments> Comments { get; set; }
        public DbSet<CommentLikes> CommentLikes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite key for UserManga
            modelBuilder.Entity<UserManga>()
                .HasKey(um => um.MangaId);  // Unique identifier for UserManga

            // Configure User - UserManga relationship
            modelBuilder.Entity<UserManga>()
                .HasOne(um => um.User)
                .WithMany(u => u.UserMangas)
                .HasForeignKey(um => um.UserId); 

            // Configure User - Comments relationship
            modelBuilder.Entity<User>()
                .HasMany(U => U._Comments)
                .WithOne(C => C.User)
                .HasForeignKey(c => c.UserId);
           

            // Configure composite key for CommentLike
            modelBuilder.Entity<CommentLikes>()
                .HasKey(cl => cl.CommentLikeId);
            modelBuilder.Entity<Comments>()
                .HasMany(c=>c.CommentLikes)
                .WithOne(cl=>cl.Comment)
                .HasForeignKey(c=>c.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<User>()
                .HasMany(u=>u.CommentLikes)
                .WithOne(cl=>cl.User)
                .HasForeignKey(u=>u.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}