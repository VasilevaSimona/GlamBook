using GlamBook.Domain.Common;
using GlamBook.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public AppointmentStatus Status { get; set; }= AppointmentStatus.Pending;
        public int SalonId { get; set; }
        public Salon Salon { get; set; } = default!;
        public int ServiceId { get; set; }
        public Service Service { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public AppUser User { get; set; } = default!;

    }
}
