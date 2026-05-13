using Microsoft.AspNetCore.Mvc;

namespace GlamBook.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // TODO: Validate credentials using Identity later 
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string email, string password)
        {
            // TODO: Create user in Identity
            return RedirectToAction("Login");
        }
    }
}
