using System.ComponentModel.DataAnnotations;

namespace SIA_2026_KIM.Web.Data.Entities
{
    public class Event
    {

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        [Required, MaxLength(150)]
        public string Venue { get; set; }

        [Required, MaxLength(50)]
        public string Details { get; set; }
    }

}
