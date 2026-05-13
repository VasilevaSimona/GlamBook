using GlamBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlamBook.Infrastructure.Data.Configurations
{
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> b)
        {
            b.Property(x => x.Stars).IsRequired();

            b.HasOne(x => x.Salon)
             .WithMany(s => s.Ratings)
             .HasForeignKey(x => x.SalonId)
             .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(x => x.User)
             .WithMany(u => u.Ratings)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);

            // (Not unique: allow multiple reviews over time if you want.
            // If you want at most one rating per user per salon, make it unique.)
            b.HasIndex(x => new { x.SalonId, x.UserId }).IsUnique(false);
        }
    }
}
