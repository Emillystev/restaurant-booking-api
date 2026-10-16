using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookingSystem.Infrastructure.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> b)
    {
        b.HasKey(x => x.Id);
        b.Property(x => x.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(x => x.SpecialRequests).HasMaxLength(500);

        b.HasIndex(x => new { x.TableId, x.StartTime, x.EndTime });
        b.HasIndex(x => new { x.CustomerId, x.StartTime });

        b.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Reservations_Time", "[EndTime] > [StartTime]");
            t.HasCheckConstraint("CK_Reservations_Guests", "[GuestsCount] > 0");
        });

        b.HasOne(x => x.Customer)
         .WithMany(u => u.Reservations)
         .HasForeignKey(x => x.CustomerId)
         .OnDelete(DeleteBehavior.Restrict);

        b.HasQueryFilter(r => !r.Table.Zone.Restaurant.IsDeleted);
    }
}