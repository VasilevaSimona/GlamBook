using GlamBook.Application.DTOs;
using GlamBook.Domain.Entities;
using GlamBook.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GlamBook.Web.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class ServicesController : Controller
    {
        private readonly GlamBookDbContext _db;
        private readonly UserManager<AppUser> _userManager;

        public ServicesController(GlamBookDbContext db, UserManager<AppUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        private async Task<Domain.Entities.Salon?> GetManagerSalonAsync()
        {
            var userId = _userManager.GetUserId(User);
            return await _db.Salons
                .Include(s => s.Services)
                .FirstOrDefaultAsync(s => s.ManagerUserId == userId);
        }

        public async Task<IActionResult> Index()
        {
            var salon = await GetManagerSalonAsync();
            if (salon == null) return NotFound();
            ViewBag.SalonName = salon.Name;
            return View(salon.Services.ToList());
        }

        public async Task<IActionResult> Create()
        {
            var salon = await GetManagerSalonAsync();
            if (salon == null) return NotFound();
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.SalonId = salon.Id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ServiceDto dto)
        {
            var salon = await GetManagerSalonAsync();
            if (salon == null) return NotFound();

            var service = new Domain.Entities.Service
            {
                Name = dto.Name,
                Price = dto.Price,
                Duration = TimeSpan.FromMinutes(dto.DurationInMinutes),
                CategoryId = dto.CategoryId,
                SalonId = salon.Id
            };

            _db.Services.Add(service);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var salon = await GetManagerSalonAsync();
            if (salon == null) return NotFound();

            var service = await _db.Services.FindAsync(id);
            if (service == null || service.SalonId != salon.Id) return NotFound();

            _db.Services.Remove(service);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}