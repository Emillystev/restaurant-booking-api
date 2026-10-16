using BookingSystem.Application.Tables.DTOs;
using BookingSystem.Infrastructure;

namespace BookingSystem.Application.Tables;

public class TableService : ITableService
{
    private readonly AppDbContext _db;
    public TableService(AppDbContext db)
    {
        _db = db;   
    }
    public Task<TableResponse> CreateAsync(CreateTableRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task DeactivateAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Guid id, UpdateTableRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
