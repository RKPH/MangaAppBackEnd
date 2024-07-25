using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MangaApp.Model.Domain
{
    public class UserManga
    {
        [Key]
        public Guid MangaId { get; set; }

         
        public Guid UserId { get; set; }

        public string Slug { get; set; }
        public string MangaName { get; set; }
        public string MangaImage { get; set; }
        [Required]
        public SaveType SaveType { get; set; }
        
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}