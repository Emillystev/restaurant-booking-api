using BookingSystem.Application.Tables.DTOs;

namespace BookingSystem.Application.Tables;

public class TableService : ITableService
{
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
