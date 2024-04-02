using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiRestaurant.Data;
using ThaiRestaurant.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ThaiRestaurant.Controllers
{
    public class DishesController : Controller
    {
        #region Fields

        private readonly DatabaseContext _context;
        private readonly ImageUploadService _imageUploadService;

        #endregion Fields

        #region Controllers

        public DishesController(DatabaseContext context, ImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        #endregion Controllers

        #region Public Methods

        public IActionResult Index()
        {
            List<Dish> dishes = _context.GetDishes();

            return View(dishes);

        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
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
                foreach (ModelStateEntry value in ModelState.Values)
                {
                    foreach (ModelError error in value.Errors)
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
            Dish dish = _context.GetDishById(id);

            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }



        [Authorize]
        [HttpPost]
        public IActionResult Edit(int id, Dish dish, IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.Remove("imageFile");
            }

            if (ModelState.IsValid)
            {
                Dish existingDish = _context.GetDishById(id);

                if (existingDish == null)
                {
                    return NotFound();
                }

                if (imageFile != null && imageFile.Length > 0)
                {
                    existingDish.ImageUrl = _imageUploadService.UploadImage(imageFile); // Update ImageUrl only if a new file is provided
                }

                existingDish.Name = dish.Name.Trim();
                existingDish.Description = dish.Description.Trim();
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
            Dish dish = _context.GetDishById(id);

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

        #endregion Public Methods
    }
}