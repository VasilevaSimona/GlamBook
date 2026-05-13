using GlamBook.Application.Interfaces;
using GlamBook.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GlamBook.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AppointmentsController : Controller
    {
        private readonly IAppointmentService _appointments;

        public AppointmentsController(IAppointmentService appointments)
        {
            _appointments = appointments;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _appointments.GetAllAsync();
            return View(list);
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