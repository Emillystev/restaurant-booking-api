using BookingSystem.Domain.Common;
using BookingSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection.Metadata;

namespace BookingSystem.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users {  get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Zone> Zones {  get; set; }
    public DbSet<RestaurantTable> Tables {  get; set; }
    public DbSet<Reservation> Reservations {  get; set; }
    public DbSet<ClosedDate> ClosedDates {  get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.HasDefaultSchema("dbo");

        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Restaurant>().ToTable("restaurants");
        modelBuilder.Entity<Zone>().ToTable("zones");
        modelBuilder.Entity<RestaurantTable>().ToTable("tables");
        modelBuilder.Entity<Reservation>().ToTable("reservations");
        modelBuilder.Entity<ClosedDate>().ToTable("closed_dates");
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedAt = now;
            else if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = now;
        }

        return base.SaveChangesAsync(ct);
    }
}