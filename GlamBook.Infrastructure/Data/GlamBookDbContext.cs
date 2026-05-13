using GlamBook.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GlamBook.Infrastructure.Data
{
    public class GlamBookDbContext : IdentityDbContext<AppUser>
    {

        public GlamBookDbContext(DbContextOptions<GlamBookDbContext> options) : base(options) { }

        public DbSet<Salon> Salons => Set<Salon>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Appointment> Appointments => Set<Appointment>();
        public DbSet<Rating> Ratings => Set<Rating>();
        public DbSet<BlogPost> BlogPosts => Set<BlogPost>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
