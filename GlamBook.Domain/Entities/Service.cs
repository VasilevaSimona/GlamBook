using GlamBook.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Domain.Entities
{
    public class Service : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public TimeSpan Duration { get; set; }
        public int SalonId { get; set; }
        public Salon Salon { get; set; } = default!;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
