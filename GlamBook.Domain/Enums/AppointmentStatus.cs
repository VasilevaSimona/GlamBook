using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Domain.Enums
{
    public enum AppointmentStatus
    {
        Pending=0,
        Confirmed=1,
        Declined=2,
        Completed=3,
        Cancelled= 4
    }
}
