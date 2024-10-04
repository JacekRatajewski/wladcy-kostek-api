using System.ComponentModel.DataAnnotations;

namespace WladcyKostek.Repo.Entities
{
    public class News
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
        public string? ImageBase64 { get; set; }
        public string? UserId { get; set; }
        public DateTime? DateTime { get; set; }
        public bool Sent { get; set; }
        public string? VideoUrl { get; set; }
    }
}
