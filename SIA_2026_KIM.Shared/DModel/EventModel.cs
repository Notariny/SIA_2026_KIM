using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SIA_2026_KIM.Shared.DModel
{
    public class EventModel
    {

        [Required]
        public string Title { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }
        [Required]
        public string Venue { get; set; }

        [Required]
        public string Details { get; set; }
    }
}
