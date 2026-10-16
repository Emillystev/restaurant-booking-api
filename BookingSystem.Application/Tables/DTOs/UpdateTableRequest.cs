namespace BookingSystem.Application.Tables.DTOs;

public class UpdateTableRequest
{
    public Guid ZoneId { get; set; }
    public int Number { get; set; }
    public int Capacity { get; set; }
    public bool IsActive { get; set; } = true;
    public byte[]? RowVersion { get; set; }
}
