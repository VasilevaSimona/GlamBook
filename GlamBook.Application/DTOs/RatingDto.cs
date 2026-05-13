using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Application.DTOs
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int Stars { get; set; }
        public string? Comment { get; set; }
        public int SalonId { get; set; }
        public string UserId { get; set; } = default!;
        
    }
}
