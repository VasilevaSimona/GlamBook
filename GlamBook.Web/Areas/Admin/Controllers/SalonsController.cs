using GlamBook.Application.DTOs;
using GlamBook.Application.Interfaces;
using GlamBook.Domain.Entities;
using GlamBook.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GlamBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SalonsController : Controller
    {
        private readonly ISalonService _salons;
        private readonly GlamBookDbContext _db;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<AppUser> _userManager;


        public SalonsController(ISalonService salons, GlamBookDbContext db, IWebHostEnvironment env, UserManager<AppUser> userManager)
        {
            _salons = salons;
            _db = db;
            _env = env;
            _userManager = userManager;

        }

        public async Task<IActionResult> Index()
        {
            var list = await _salons.GetAllAsync();
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Users = await _db.Users.ToListAsync();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(SalonDto dto, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.ToListAsync();
                ViewBag.Users = await _db.Users.ToListAsync();
                return View(dto);
            }

            if (image != null && image.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string path = Path.Combine(_env.WebRootPath, "images/salons", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await image.CopyToAsync(stream);
                dto.ImageUrl = $"/images/salons/{fileName}";
            }

            await _salons.CreateAsync(dto);

            // Assign Manager role to selected user
            if (!string.IsNullOrEmpty(dto.ManagerUserId))
            {
                var user = await _userManager.FindByIdAsync(dto.ManagerUserId);
                if (user != null && !await _userManager.IsInRoleAsync(user, "Manager"))
                    await _userManager.AddToRoleAsync(user, "Manager");
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _salons.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _salons.GetAsync(id);
            if (dto == null) return NotFound();
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Users = await _db.Users.ToListAsync();
            return View(dto);
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Edit(SalonDto dto, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.ToListAsync();
                ViewBag.Users = await _db.Users.ToListAsync();
                return View(dto);
            }

            if (image != null && image.Length > 0)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(image.FileName);
                string path = Path.Combine(_env.WebRootPath, "images/salons", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await image.CopyToAsync(stream);
                dto.ImageUrl = "/images/salons/" + fileName;
            }

            // Remove Manager role from old manager if changed
            var oldSalon = await _db.Salons.FindAsync(dto.Id);
            if (oldSalon?.ManagerUserId != null && oldSalon.ManagerUserId != dto.ManagerUserId)
            {
                var oldManager = await _userManager.FindByIdAsync(oldSalon.ManagerUserId);
                if (oldManager != null)
                    await _userManager.RemoveFromRoleAsync(oldManager, "Manager");
            }

            await _salons.UpdateAsync(dto);

            // Assign Manager role to new manager
            if (!string.IsNullOrEmpty(dto.ManagerUserId))
            {
                var user = await _userManager.FindByIdAsync(dto.ManagerUserId);
                if (user != null && !await _userManager.IsInRoleAsync(user, "Manager"))
                    await _userManager.AddToRoleAsync(user, "Manager");
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
