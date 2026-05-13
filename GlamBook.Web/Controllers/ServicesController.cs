using Microsoft.AspNetCore.Mvc;
using GlamBook.Application.Interfaces;

namespace GlamBook.Web.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _services;

        public ServicesController(IServiceService services)
        {
            _services = services;
        }

        public async Task<IActionResult> Index(int salonId)
        {
            var list = await _services.GetBySalonAsync(salonId);
            return View(list);
        }

        public async Task<IActionResult> Details(int id)
        {
            var dto = await _services.GetAsync(id);
            return View(dto);
        }

        public async Task<IActionResult> Book(int id)
        {
            var dto = await _services.GetAsync(id);
            return View(dto);
        }
    }
}
