using GlamBook.Application.Interfaces;
using GlamBook.Domain.Entities;
using GlamBook.Domain.Enums;
using GlamBook.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GlamBook.Infrastructure.Services
{
    public class RatingService : IRatingService
    {
        private readonly GlamBookDbContext _db;
        public RatingService(GlamBookDbContext db) => _db = db;

        public async Task<int> CreateAsync(string userId, int salonId, int stars, string? comment)
        {
            // allow rating only after a CONFIRMED past appointment
            bool allowed = await _db.Appointments.AnyAsync(a =>
                a.UserId == userId &&
                a.SalonId == salonId &&
                a.Status == AppointmentStatus.Confirmed &&
                a.EndUtc < DateTime.UtcNow);

            if (!allowed)
                throw new InvalidOperationException("You can rate only after a confirmed past appointment.");

            var rating = new Rating
            {
                UserId = userId,
                SalonId = salonId,
                Stars = stars,
                Comment = comment
            };

            _db.Ratings.Add(rating);
            await _db.SaveChangesAsync();
            return rating.Id;
        }

        public async Task<double> GetAverageAsync(int salonId)
            => await _db.Ratings
                    .Where(r => r.SalonId == salonId)
                    .Select(r => (double)r.Stars)
                    .DefaultIfEmpty(0)
                    .AverageAsync();
    }
}
