using Microsoft.AspNetCore.Mvc;
using GlamBook.Application.Interfaces;

namespace GlamBook.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categories;

        public CategoriesController(ICategoryService categories)
        {
            _categories = categories;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _categories.GetAllAsync();
            return View(list);
        }
    }
}
