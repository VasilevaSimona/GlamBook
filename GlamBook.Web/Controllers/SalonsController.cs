using Microsoft.AspNetCore.Mvc;
using GlamBook.Application.Interfaces;

namespace GlamBook.Web.Controllers
{
    public class SalonsController : Controller
    {
        private readonly ISalonService _salons;

        public SalonsController(ISalonService salons)
        {
            _salons = salons;
        }

        public async Task<IActionResult> Index(int? categoryId)
        {
            var list = await _salons.GetAllAsync(categoryId);
            return View(list);
        }
    }
}
