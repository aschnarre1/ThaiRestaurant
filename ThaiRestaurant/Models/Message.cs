using System.ComponentModel.DataAnnotations;

namespace ThaiRestaurant.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(500)]
        public string MessageText { get; set; }
    }
}
