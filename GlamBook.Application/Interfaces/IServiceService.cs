using GlamBook.Application.DTOs;

namespace GlamBook.Application.Interfaces
{
    public interface IServiceService
    {
        Task<IEnumerable<ServiceDto>> GetAllAsync();
        Task<ServiceDto?> GetAsync(int id);
        Task CreateAsync(ServiceDto dto);
        Task UpdateAsync(ServiceDto dto);
        Task DeleteAsync(int id);
        Task<IEnumerable<ServiceDto>> GetBySalonAsync(int salonId);
    }
}
