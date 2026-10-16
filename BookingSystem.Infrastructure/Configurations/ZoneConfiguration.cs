using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Infrastructure.Configurations;

public class ZoneConfiguration : IEntityTypeConfiguration<Zone>
{
    public void Configure(EntityTypeBuilder<Zone> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.ZoneName).IsRequired().HasMaxLength(100);
        b.HasIndex(x => new { x.RestaurantId, x.ZoneName }).IsUnique();

        b.HasMany(x => x.Tables)
         .WithOne(t => t.Zone)
         .HasForeignKey(t => t.ZoneId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}