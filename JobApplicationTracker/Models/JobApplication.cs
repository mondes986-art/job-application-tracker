
using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Models
{
    public class JobApplication
    {
        public int Id { get; set; }

        [Required]
        public string Company { get; set; } = string.Empty;

        [Required]
        public string Position { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? DateApplied { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        public string? Location { get; set; }

        public string? Notes { get; set; }
    }
}
