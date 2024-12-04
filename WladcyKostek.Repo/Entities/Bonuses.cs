using System.ComponentModel.DataAnnotations;

namespace WladcyKostek.Repo.Entities
{
    public class Bonuses
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? SessionCount { get; set; }
        public int? BonusCount { get; set; }
        public int? MoneySupported { get; set; }
        public int? PlayerSeasonStart { get; set; }
        public bool? IsPublic { get; set; }
    }
}
