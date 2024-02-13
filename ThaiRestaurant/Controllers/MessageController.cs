using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ThaiRestaurant.Data;
using ThaiRestaurant.Models;

namespace ThaiRestaurant.Controllers
{
    public class MessageController : Controller
    {
        private readonly DatabaseContext _context;

        public MessageController(DatabaseContext context)
        {
            _context = context;
        }


        [Authorize]
        public IActionResult Index()
        {
            var messages = _context.GetMessages();
            return View(messages);
        }


        [HttpPost]
        public IActionResult Create(Message message)
        {
            if (ModelState.IsValid)
            {
                _context.CreateMessage(message);
                return RedirectToAction("Index", "Home");
            }


            return RedirectToAction("ContactUs", "Home");
        }



        public IActionResult Delete(int id)
        {
            var message = _context.GetMessageById(id);
            if (message == null)
            {
                return NotFound();
            }
            return View(message);
        }

        [Authorize]
        public IActionResult DeleteMessage(int id)
        {
            _context.DeleteMessage(id);
            return RedirectToAction("Index");
        }


    }
}
