using BookingSystem.Application.Tables.DTOs;

namespace BookingSystem.Application.Tables;

public interface ITableService
{
    Task<TableResponse> CreateAsync(CreateTableRequest request, CancellationToken ct = default);
    Task UpdateAsync(Guid id, UpdateTableRequest request, CancellationToken ct = default);
    Task DeactivateAsync(Guid id, CancellationToken ct = default);
}
