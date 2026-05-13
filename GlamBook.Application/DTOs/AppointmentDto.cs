namespace GlamBook.Application.DTOs
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public string SalonName { get; set; } = "";
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = "";
        public DateTime StartUtc { get; set; }
        public DateTime EndUtc { get; set; }
        public string Status { get; set; } = "";
        public string UserEmail { get; set; } = ""; 

    }
}
