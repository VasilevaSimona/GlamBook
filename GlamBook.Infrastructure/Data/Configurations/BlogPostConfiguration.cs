using GlamBook.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlamBook.Infrastructure.Data.Configurations
{
    public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
    {
        public void Configure(EntityTypeBuilder<BlogPost> b)
        {
            b.Property(x => x.Title).IsRequired().HasMaxLength(200);
            b.Property(x => x.CoverUrl).HasMaxLength(1000);

        }
    }
}
