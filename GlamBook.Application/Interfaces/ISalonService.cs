using GlamBook.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Application.Interfaces
{
    public interface ISalonService
    {
        Task<IEnumerable<SalonDto>> GetAllAsync(int? categoryId = null);
        Task<SalonDto?> GetAsync(int id);
        Task<int> CreateAsync(SalonDto model);
        Task UpdateAsync(SalonDto model);
        Task DeleteAsync(int id);
    }
}
