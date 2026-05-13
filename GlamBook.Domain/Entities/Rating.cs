using GlamBook.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Domain.Entities
{
    public class Rating : BaseEntity
    {
        public int Stars { get; set; }
        public string? Comment { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public AppUser User { get; set; } = default!;
    }

}
