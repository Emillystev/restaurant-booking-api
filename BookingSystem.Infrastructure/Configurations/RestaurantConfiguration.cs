using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Infrastructure.Configurations;

public class RestaurantConfiguration : IEntityTypeConfiguration<Restaurant>
{
    public void Configure(EntityTypeBuilder<Restaurant> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).IsRequired().HasMaxLength(150);
        b.Property(x => x.Address).IsRequired().HasMaxLength(300);
        b.Property(x => x.Phone).HasMaxLength(30);
        b.HasQueryFilter(x => !x.IsDeleted);

        b.HasMany(x => x.Zones)
         .WithOne(z => z.Restaurant)
         .HasForeignKey(z => z.RestaurantId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasMany(x => x.ClosedDates)
         .WithOne(c => c.Restaurant)
         .HasForeignKey(c => c.RestaurantId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}