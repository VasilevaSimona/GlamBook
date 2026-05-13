using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Application.Interfaces
{
    public interface IRatingService
    {
        Task<int> CreateAsync(string userId, int salonId, int stars, string? comment);
        Task<double> GetAverageAsync(int salonId);
    }
}
