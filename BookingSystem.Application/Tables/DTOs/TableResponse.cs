namespace BookingSystem.Application.Tables.DTOs;

public class TableResponse
{
    public Guid Id { get; set; }
    public Guid ZoneId { get; set; }
    public int Number { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; }
    public byte[]? RowVersion { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
