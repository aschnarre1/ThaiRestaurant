using System.ComponentModel.DataAnnotations;

namespace ThaiRestaurant.Models
{
    public class Dish
    {
        [Key]
        public int DishId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must be less than 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(00, ErrorMessage = "Description must be less than 500 characters")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(300)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        public bool IsFeatured { get; set; }
    }
}
