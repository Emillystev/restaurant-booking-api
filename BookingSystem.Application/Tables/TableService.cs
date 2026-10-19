using BookingSystem.Application.Tables.DTOs;
using BookingSystem.Domain.Entities;
using BookingSystem.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BookingSystem.Application.Tables;

public class TableService : ITableService
{
    private readonly AppDbContext _db;
    public TableService(AppDbContext db)
    {
        _db = db;   
    }
    public async Task<TableResponse> CreateAsync(CreateTableRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var table = new RestaurantTable()
        {
            ZoneId = request.ZoneId,
            Number = request.Number,
            Capacity = request.Capacity,
            IsActive = request.IsActive,
        };

        _db.Tables.Add(table);
        await _db.SaveChangesAsync(ct);

        return new TableResponse()
        {
            Id = table.Id,
            ZoneId = table.ZoneId,
            Number = table.Number,
            Capacity = table.Capacity,
            IsActive = table.IsActive,
            RowVersion = table.RowVersion,
            CreatedAt = table.CreatedAt,
            UpdatedAt = table.UpdatedAt
        };
    }

    public async Task DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        var table = await _db.Tables
            .SingleOrDefaultAsync(table => table.Id == id, ct)
            ?? throw new KeyNotFoundException($"Table with id '{id}' was not found.");

        table.IsActive = false;

        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Guid id, UpdateTableRequest request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(request.Capacity);

        var table = await _db.Tables
            .SingleOrDefaultAsync(table => table.Id == id, ct)
            ?? throw new KeyNotFoundException($"Table with id '{id}' was not found.");

        if (!await _db.Zones.AnyAsync(zone => zone.Id == request.ZoneId, ct))
        {
            throw new KeyNotFoundException($"Zone with id '{request.ZoneId}' was not found.");
        }

        if (await _db.Tables.AnyAsync(other =>
            other.ZoneId == request.ZoneId &&
            other.Number == request.Number &&
            other.Id != id, ct))
        {
            throw new InvalidOperationException(
                $"Table number '{request.Number}' already exists in this zone.");
        }

        if (request.RowVersion is not null)
        {
            _db.Entry(table).Property(table => table.RowVersion).OriginalValue = request.RowVersion;
        }

        table.ZoneId = request.ZoneId;
        table.Number = request.Number;
        table.Capacity = request.Capacity;
        table.IsActive = request.IsActive;

        await _db.SaveChangesAsync(ct);
    }
}
