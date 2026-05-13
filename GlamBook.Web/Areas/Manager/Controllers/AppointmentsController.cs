using GlamBook.Application.Interfaces;
using GlamBook.Domain.Enums;
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
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointments;
        private readonly UserManager<AppUser> _userManager;
        private readonly GlamBookDbContext _db;

        public AppointmentsController(IAppointmentService appointments, UserManager<AppUser> userManager, GlamBookDbContext db)
        {
            _appointments = appointments;
            _userManager = userManager;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var salon = await _db.Salons.FirstOrDefaultAsync(s => s.ManagerUserId == userId);
            if (salon == null) return NotFound();

            var appointments = await _appointments.ForSalonAsync(salon.Id);
            ViewBag.SalonName = salon.Name;
            return View(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int id)
        {
            await _appointments.UpdateStatusAsync(id, AppointmentStatus.Confirmed);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            await _appointments.UpdateStatusAsync(id, AppointmentStatus.Cancelled);
            return RedirectToAction(nameof(Index));
        }
    }
}