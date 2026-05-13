using AutoMapper;
using GlamBook.Application.DTOs;
using GlamBook.Application.Interfaces;
using GlamBook.Domain.Entities;
using GlamBook.Domain.Enums;
using GlamBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GlamBook.Infrastructure.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly GlamBookDbContext _db;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public AppointmentService(GlamBookDbContext db, IMapper mapper, IEmailService emailService)
        {
            _db = db;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<int> BookAsync(string userId, int salonId, int serviceId, DateTime startUtc)
        {
            var service = await _db.Services
                .FirstOrDefaultAsync(s => s.Id == serviceId && s.SalonId == salonId)
                ?? throw new InvalidOperationException("Service not found in this salon.");

            var appt = new Appointment
            {
                UserId = userId,
                SalonId = salonId,
                ServiceId = serviceId,
                StartUtc = startUtc,
                EndUtc = startUtc.Add(service.Duration),
                Status = AppointmentStatus.Pending
            };

            _db.Appointments.Add(appt);
            await _db.SaveChangesAsync();
            return appt.Id;
        }

        public async Task CancelAsync(int appointmentId, string userId)
        {
            var appt = await _db.Appointments
                .Include(a => a.Service)
                .Include(a => a.Salon)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == appointmentId)
                ?? throw new KeyNotFoundException();

            if (appt.UserId != userId) throw new UnauthorizedAccessException();

            appt.Status = AppointmentStatus.Cancelled;
            await _db.SaveChangesAsync();

            try
            {
                if (appt.User?.Email != null)
                {
                    var time = appt.StartUtc;
                    var html = $@"
                    <div style='font-family: Montserrat, sans-serif; max-width: 600px; margin: auto;'>
                        <h1 style='font-family: Georgia, serif; color: #e91e8c;'>Booking Cancelled</h1>
                        <p>Hi {appt.User.Email},</p>
                        <p>You have successfully cancelled your appointment:</p>
                        <div style='background: #fdf7f4; border-radius: 12px; padding: 24px; margin: 20px 0;'>
                            <p><strong>Service:</strong> {appt.Service.Name}</p>
                            <p><strong>Salon:</strong> {appt.Salon.Name}</p>
                            <p><strong>Date:</strong> {time:dddd, dd MMMM yyyy}</p>
                            <p><strong>Time:</strong> {time:HH:mm}</p>
                        </div>
                        <p>You can book a new appointment anytime on GlamBook.</p>
                        <p style='color: #e91e8c; font-weight: bold;'>— The GlamBook Team</p>
                    </div>";

                    await _emailService.SendAsync(appt.User.Email, "Your GlamBook Booking has been Cancelled", html);
                }
            }
            catch
            {
                // Email failed silently — cancellation already saved
            }
        }

        public async Task ConfirmAsync(int appointmentId, string managerUserId)
        {
            var appt = await _db.Appointments
                .Include(a => a.Salon)
                .FirstOrDefaultAsync(a => a.Id == appointmentId)
                ?? throw new KeyNotFoundException();

            if (appt.Salon.ManagerUserId != managerUserId)
                throw new UnauthorizedAccessException();

            appt.Status = AppointmentStatus.Confirmed;
            await _db.SaveChangesAsync();
        }

        public async Task DeclineAsync(int appointmentId, string managerUserId)
        {
            var appt = await _db.Appointments
                .Include(a => a.Salon)
                .FirstOrDefaultAsync(a => a.Id == appointmentId)
                ?? throw new KeyNotFoundException();

            if (appt.Salon.ManagerUserId != managerUserId)
                throw new UnauthorizedAccessException();

            appt.Status = AppointmentStatus.Declined;
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppointmentDto>> ForUserAsync(string userId)
        {
            var list = await _db.Appointments
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.StartUtc)
                .ToListAsync();

            return _mapper.Map<IEnumerable<AppointmentDto>>(list);
        }

        public async Task<IEnumerable<AppointmentDto>> ForSalonAsync(int salonId)
        {
            return await _db.Appointments
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Include(a => a.User)
                .Where(a => a.SalonId == salonId)
                .OrderByDescending(a => a.StartUtc)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    SalonId = a.SalonId,
                    SalonName = a.Salon.Name,
                    ServiceId = a.ServiceId,
                    ServiceName = a.Service.Name,
                    StartUtc = a.StartUtc,
                    EndUtc = a.EndUtc,
                    Status = a.Status.ToString(),
                    UserEmail = a.User.Email ?? ""
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
        {
            return await _db.Appointments
                .Include(a => a.Salon)
                .Include(a => a.Service)
                .Include(a => a.User)
                .OrderByDescending(a => a.StartUtc)
                .Select(a => new AppointmentDto
                {
                    Id = a.Id,
                    SalonId = a.SalonId,
                    SalonName = a.Salon.Name,
                    ServiceId = a.ServiceId,
                    ServiceName = a.Service.Name,
                    StartUtc = a.StartUtc,
                    EndUtc = a.EndUtc,
                    Status = a.Status.ToString(),
                    UserEmail = a.User.Email ?? ""
                })
                .ToListAsync();
        }

        public async Task UpdateStatusAsync(int id, AppointmentStatus status)
        {
            var appt = await _db.Appointments
                .Include(a => a.Service)
                .Include(a => a.Salon)
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == id)
                ?? throw new KeyNotFoundException();

            appt.Status = status;
            await _db.SaveChangesAsync();

            try
            {
                if (status == AppointmentStatus.Confirmed && appt.User?.Email != null)
                {
                    var time = appt.StartUtc;
                    var html = $@"
                    <div style='font-family: Montserrat, sans-serif; max-width: 600px; margin: auto;'>
                        <h1 style='font-family: Georgia, serif; color: #e91e8c;'>Booking Confirmed!</h1>
                        <p>Hi {appt.User.Email},</p>
                        <p>Your appointment has been confirmed. Here are your details:</p>
                        <div style='background: #fdf7f4; border-radius: 12px; padding: 24px; margin: 20px 0;'>
                            <p><strong>Service:</strong> {appt.Service.Name}</p>
                            <p><strong>Salon:</strong> {appt.Salon.Name}</p>
                            <p><strong>Address:</strong> {appt.Salon.Address}</p>
                            <p><strong>Date:</strong> {time:dddd, dd MMMM yyyy}</p>
                            <p><strong>Time:</strong> {time:HH:mm}</p>
                        </div>
                        <p>See you soon!</p>
                        <p style='color: #e91e8c; font-weight: bold;'>— The GlamBook Team</p>
                    </div>";

                    await _emailService.SendAsync(appt.User.Email, "Your GlamBook Appointment is Confirmed!", html);
                }

                if (status == AppointmentStatus.Cancelled && appt.User?.Email != null)
                {
                    var time = appt.StartUtc;
                    var html = $@"
                    <div style='font-family: Montserrat, sans-serif; max-width: 600px; margin: auto;'>
                        <h1 style='font-family: Georgia, serif; color: #e91e8c;'>Appointment Cancelled</h1>
                        <p>Hi {appt.User.Email},</p>
                        <p>Unfortunately your appointment has been cancelled. Here were your booking details:</p>
                        <div style='background: #fdf7f4; border-radius: 12px; padding: 24px; margin: 20px 0;'>
                            <p><strong>Service:</strong> {appt.Service.Name}</p>
                            <p><strong>Salon:</strong> {appt.Salon.Name}</p>
                            <p><strong>Date:</strong> {time:dddd, dd MMMM yyyy}</p>
                            <p><strong>Time:</strong> {time:HH:mm}</p>
                        </div>
                        <p>We're sorry for the inconvenience. You can book a new appointment anytime on GlamBook.</p>
                        <p style='color: #e91e8c; font-weight: bold;'>— The GlamBook Team</p>
                    </div>";

                    await _emailService.SendAsync(appt.User.Email, "Your GlamBook Appointment has been Cancelled", html);
                }
            }
            catch
            {

            }
        }
    }
}