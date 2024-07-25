using MangaApp.Model.Domain;

namespace MangaApp.DTO;

public class mangaDto
{
  
   
    public string Slug { get; set; }
    public string MangaName { get; set; }
    public string MangaImage { get; set; }
    public SaveType SaveType { get; set; }
}