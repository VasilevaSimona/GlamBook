using GlamBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlamBook.Infrastructure.Data.Configurations
{
    public class SalonConfiguration : IEntityTypeConfiguration<Salon>
    {
        public void Configure(EntityTypeBuilder<Salon> b)
        {
            b.Property(x => x.Name).IsRequired().HasMaxLength(200);
            b.Property(x => x.Address).IsRequired().HasMaxLength(300);
            b.Property(x => x.ImageUrl).HasMaxLength(1000);

            b.HasMany(x => x.Services)
             .WithOne(s => s.Salon)
             .HasForeignKey(s => s.SalonId)
             .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
