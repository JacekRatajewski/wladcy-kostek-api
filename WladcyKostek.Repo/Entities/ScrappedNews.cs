using System.ComponentModel.DataAnnotations;

namespace WladcyKostek.Repo.Entities
{
    public class ScrappedNews
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Base64Img { get; set; }
        public string? Url { get; set; }
        public DateTime? ScrappedTime { get; set; }
    }
}
