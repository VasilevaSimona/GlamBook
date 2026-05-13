using GlamBook.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Domain.Entities
{
    public class Salon : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string? Description { get; set; } = default!;
        public string? ImageUrl { get; set; }

        public string? ManagerUserId { get; set; }
        public ICollection<Service> Services { get; set; } = new List<Service>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}
