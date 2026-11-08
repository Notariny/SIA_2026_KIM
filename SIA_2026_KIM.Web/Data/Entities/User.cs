using System.ComponentModel.DataAnnotations;

namespace SIA_2026_KIM.Web.Data.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(150)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

    }
}
