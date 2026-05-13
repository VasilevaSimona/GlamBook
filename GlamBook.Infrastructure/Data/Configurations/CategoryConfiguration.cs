using GlamBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlamBook.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> b)
        {
            b.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

            b.HasIndex(x => x.Name).IsUnique();

            b.HasData(
                new Category { Id = 1, Name = "Face" },
                new Category { Id = 2, Name = "Body" },
                new Category { Id = 3, Name = "Hair" },
                new Category { Id = 4, Name = "Hair Removal" }
            );
        }
    }
}
