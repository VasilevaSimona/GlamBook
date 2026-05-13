using GlamBook.Application.Interfaces;
using GlamBook.Web.Models;
using Microsoft.AspNetCore.Mvc;
using GlamBook.Web.Models;


namespace GlamBook.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryService _categories;
        private readonly ISalonService _salons;

        public HomeController(ICategoryService categories, ISalonService salons)
        {
            _categories = categories;
            _salons = salons;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeViewModel
            {
                Categories = await _categories.GetAllAsync(),
                Salons = await _salons.GetAllAsync()
            };

            return View(model);
        }
    }
}
