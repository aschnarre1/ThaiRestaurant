using System.ComponentModel.DataAnnotations;

namespace ThaiRestaurant.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Message is required")]
        [StringLength(500, ErrorMessage = "Message must be less than 500 characters")]
        public string MessageText { get; set; }
    }
}
