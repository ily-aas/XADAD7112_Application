using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static XADAD7112_Application.Models.System.Enums;

namespace XADAD7112_Application.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string FullName { get; set; }

        [Required, MaxLength(50)]
        public string Username { get; set; }

        [Required, MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(10)]
        public string? MobileNo { get; set; }

        [MaxLength(50)]
        public string? Email { get; set; }

        [MaxLength(250)]
        public string? Address { get; set; }

        [Required]
        public int UserRole { get; set; } = 0;

        public DateTime? DateCreated { get; set; }

        [NotMapped]
        public bool HasErrors { get; set; }

        [NotMapped]
        public string? Message { get; set; }

        //public ICollection<Product>? Products { get; set; }
    }

}
