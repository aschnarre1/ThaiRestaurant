using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ThaiRestaurant.Models;
using ThaiRestaurant.Data; 

namespace ThaiRestaurant.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DatabaseContext _context;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }
        //test
        //testing 2
        public IActionResult Index()
        {
            var dishes = _context.GetDishes(); 
            return View(dishes); 
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
