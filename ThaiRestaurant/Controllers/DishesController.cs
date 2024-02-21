using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http; 
using ThaiRestaurant.Data;
using ThaiRestaurant.Models;
using Google.Protobuf;
namespace ThaiRestaurant.Controllers
{
    public class DishesController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ImageUploadService _imageUploadService;


        public DishesController(DatabaseContext context, ImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }


        public IActionResult Index()
        {
            var dishes = _context.GetDishes();
            return View(dishes);

        }
        [Authorize]

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Dish dish, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                dish.ImageUrl = _imageUploadService.UploadImage(imageFile);
            }


            ModelState.Remove("ImageUrl"); // Remove ModelState errors related to ImageUrl

            if (ModelState.IsValid)
            {
                dish.Name = dish.Name.Trim();
                dish.Description = dish.Description.Trim();
                _context.AddDish(dish, imageFile);
                return RedirectToAction("Index");
            }

            else
            {
                // Log or inspect the validation errors
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        Console.WriteLine(error.ErrorMessage); // Log the error message
                    }
                }
                return View(dish);
            }
        }



        [Authorize]

        public IActionResult Edit(int id)
        {
            var dish = _context.GetDishById(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }



        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, Dish dish, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.Remove("imageFile");
            }

            if (ModelState.IsValid)
            {
                var existingDish = _context.GetDishById(id);
                if (existingDish == null)
                {
                    return NotFound();
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    existingDish.ImageUrl = _imageUploadService.UploadImage(imageFile); // Update ImageUrl only if a new file is provided
                }

                dish.Name = dish.Name.Trim();
                dish.Description = dish.Description.Trim();
                existingDish.Name = dish.Name;
                existingDish.Description = dish.Description;
                existingDish.Price = dish.Price;
                existingDish.IsFeatured = dish.IsFeatured;

                _context.EditDish(existingDish);
                return RedirectToAction("Index");
            }
            return View(dish);
        }




        [Authorize]

        public IActionResult Delete(int id)
        {
            var dish = _context.GetDishById(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        [Authorize]
        public IActionResult DeleteData(int id) 
        {
            _context.DeleteDish(id);
            return RedirectToAction("Index");
        }

    }
}
