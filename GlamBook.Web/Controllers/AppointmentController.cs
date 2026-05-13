using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GlamBook.Application.Interfaces;
using GlamBook.Domain.Entities;

namespace GlamBook.Web.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointments;
        private readonly ISalonService _salons;
        private readonly IServiceService _services;
        private readonly UserManager<AppUser> _userManager;

        public AppointmentController(
            IAppointmentService appointments,
            ISalonService salons,
            IServiceService services,
            UserManager<AppUser> userManager)
        {
            _appointments = appointments;
            _salons = salons;
            _services = services;
            _userManager = userManager;
        }

        // SHOW MY BOOKINGS
        public async Task<IActionResult> My()
        {
            var userId = _userManager.GetUserId(User);

            var list = await _appointments.ForUserAsync(userId);
            return View(list);
        }

        // DISPLAY BOOKING PAGE
        public async Task<IActionResult> Book(int salonId, int serviceId)
        {
            var service = await _services.GetAsync(serviceId);
            var salon = await _salons.GetAsync(salonId);
            if (service == null || salon == null)
                return NotFound();

            var allAppointments = await _appointments.ForSalonAsync(salonId);
            var bookedSlots = allAppointments
                .Where(a => a.Status != "Cancelled")
                .Select(a => a.StartUtc.ToString("yyyy-MM-dd HH:mm"))
                .ToList();

            // Debug: remove after testing
            ViewBag.Debug = string.Join(", ", bookedSlots);

            ViewBag.Salon = salon;
            ViewBag.BookedSlots = bookedSlots;
            return View(service);
        }

        // HANDLE BOOKING SUBMIT
        [HttpPost]
        public async Task<IActionResult> Book(int salonId, int serviceId, DateTime date, TimeSpan time)
        {
            var userId = _userManager.GetUserId(User);

            var startUtc = DateTime.SpecifyKind(date.Date + time, DateTimeKind.Utc);

            await _appointments.BookAsync(userId, salonId, serviceId, startUtc);

            return RedirectToAction("My");
        }

        // CANCEL BOOKING
        public async Task<IActionResult> Cancel(int id)
        {
            var userId = _userManager.GetUserId(User);
            await _appointments.CancelAsync(id, userId);
            return RedirectToAction("My");
        }
    }
}
