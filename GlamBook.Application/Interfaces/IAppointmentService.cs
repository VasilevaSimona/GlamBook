using GlamBook.Application.DTOs;
using GlamBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Application.Interfaces
{
    public interface IAppointmentService
    {
        Task<int> BookAsync(string userId, int salonId, int serviceId, DateTime startUtc);
        Task CancelAsync(int appointmentId, string userId);
        Task ConfirmAsync(int appointmentId, string managerUserId);
        Task DeclineAsync(int appointmentId, string managerUserId);
        Task<IEnumerable<AppointmentDto>> ForUserAsync(string userId);
        Task<IEnumerable<AppointmentDto>> ForSalonAsync(int salonId);
        Task<IEnumerable<AppointmentDto>> GetAllAsync();
        Task UpdateStatusAsync(int id, AppointmentStatus status);
    }
}
