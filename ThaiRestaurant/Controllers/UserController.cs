using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ThaiRestaurant.Data;
using ThaiRestaurant.Models;
using Microsoft.AspNetCore.Authorization;

namespace ThaiRestaurant.Controllers
{
    public class UserController : Controller
    {
        private readonly DatabaseContext _context;

        public UserController(DatabaseContext context)
        {
            _context = context;
        }

        [Authorize]
        public IActionResult Index()
        {
            var users = _context.GetUsers();
            return View(users);
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                user.UserName = user.UserName.Trim();
                user.Password = user.Password.Trim();
                _context.AddUser(user);
                await SetAuthCookie(user);
                return RedirectToAction("Index", "User");
            }
            return View(user);
        }


        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (ModelState.IsValid)
            {
                user.UserName = user.UserName.Trim();
                user.Password = user.Password.Trim();

                var loggedInUser = _context.LoginUser(user.UserName, user.Password);
                if (loggedInUser != null)
                {
                    await SetAuthCookie(loggedInUser);
                    return RedirectToAction("Index", "User");
                }
                return View();
            }
            return View(user);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }



        public IActionResult Delete(int id)
        {
            var user = _context.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [Authorize]
        public IActionResult DeleteUserData(int id)
        {
            _context.DeleteUser(id);
            return RedirectToAction("Index");
        }





        private async Task SetAuthCookie(User user)
        {
            var isAdmin = _context.GetUserById(user.UserId)?.isAdmin ?? false;
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Sid, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("IsAdmin", isAdmin.ToString()), 


            };

            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            ClaimsPrincipal principle = new(claimsIdentity);

            AuthenticationProperties authenticationProperties = new()
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddHours(1)
            };

            await HttpContext.SignOutAsync();
            await HttpContext.SignInAsync(principle, authenticationProperties);
        }


    }
}
