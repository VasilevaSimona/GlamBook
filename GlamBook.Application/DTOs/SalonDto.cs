namespace GlamBook.Application.DTOs
{
    public class SalonDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? ManagerUserId { get; set; }
        public int CategoryId { get; set; }
        public List<ServiceDto> Services { get; set; } = new();
    }
}