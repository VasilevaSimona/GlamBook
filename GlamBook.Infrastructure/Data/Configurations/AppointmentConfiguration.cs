using GlamBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlamBook.Infrastructure.Data.Configurations
{
    public class AppointmentConfiguration : IEntityTypeConfiguration<Appointment>
    {
        public void Configure(EntityTypeBuilder<Appointment> b)
        {
            b.Property(x => x.StartUtc).IsRequired();
            b.Property(x => x.EndUtc).IsRequired();

            b.HasOne(x => x.User)
             .WithMany(u => u.Appointments)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(x => x.Salon)
             .WithMany(s => s.Appointments)
             .HasForeignKey(x => x.SalonId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.Service)
             .WithMany(s => s.Appointments)
             .HasForeignKey(x => x.ServiceId)
             .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
