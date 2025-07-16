using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace evercloud.Domain.Models
{
    public class SupportMessage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Foreign Key (just the ID string)

        [ForeignKey("UserId")]
        public Users User { get; set; } // Navigation property (optional)

        [Required]
        public string Sender { get; set; } // "User" or "Admin"

        [Required]
        public string MessageContent { get; set; }

        public DateTime Timestamp { get; set; }

        public bool IsRead { get; set; } = false;

    }
}
