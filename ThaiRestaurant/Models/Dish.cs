using System.ComponentModel.DataAnnotations;

namespace ThaiRestaurant.Models
{
    public class Dish
    {
        [Key]
        public int DishId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [StringLength(300)]
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }
    }
}
