using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WladcyKostek.Repo.Entities
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public bool? FromGoogle { get; set; }
        public DateTime? AccountCreationDate { get; set; }
    }
}
