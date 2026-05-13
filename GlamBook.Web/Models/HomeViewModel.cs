using GlamBook.Application.DTOs;

namespace GlamBook.Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<CategoryDto> Categories { get; set; } = new List<CategoryDto>();
        public IEnumerable<SalonDto> Salons { get; set; } = new List<SalonDto>();
    }
}
