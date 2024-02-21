using System.ComponentModel.DataAnnotations;

namespace ThaiRestaurant.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Username is required")]
        [StringLength(255, ErrorMessage = "Username must be less than 255 characters")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(255, ErrorMessage = "Password must be less than 255 characters")]
        public string Password { get; set; }

        public bool isAdmin { get; set; }

    }
}
