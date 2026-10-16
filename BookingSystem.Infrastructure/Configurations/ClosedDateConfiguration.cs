using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Infrastructure.Configurations;

public class ClosedDateConfiguration : IEntityTypeConfiguration<ClosedDate>
{
    public void Configure(EntityTypeBuilder<ClosedDate> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Reason).HasMaxLength(200);
        b.HasIndex(x => new { x.RestaurantId, x.Date }).IsUnique();

        b.HasQueryFilter(c => !c.Restaurant.IsDeleted);
    }
}