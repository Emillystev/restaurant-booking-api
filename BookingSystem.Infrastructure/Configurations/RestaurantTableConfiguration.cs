using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Infrastructure.Configurations;

public class RestaurantTableConfiguration : IEntityTypeConfiguration<RestaurantTable>
{
    public void Configure(EntityTypeBuilder<RestaurantTable> b)
    {
        b.ToTable("Tables");
        b.HasKey(x => x.Id);
        b.Property(x => x.RowVersion).IsRowVersion();
        b.HasIndex(x => new { x.ZoneId, x.Number }).IsUnique();
        b.ToTable(t => t.HasCheckConstraint("CK_Tables_Capacity", "[Capacity] > 0"));

        b.HasMany(x => x.Reservations)
         .WithOne(r => r.Table)
         .HasForeignKey(r => r.TableId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}