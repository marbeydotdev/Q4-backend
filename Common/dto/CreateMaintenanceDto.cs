using Common.Enums;

namespace Common.dto;

public class CreateMaintenanceDto
{
    public DateTime PlannedDate { get; set; }
    public int MoldId { get; set; }
    public MaintenanceType MaintenanceType { get; set; }
    public string Description { get; set; } = null!;
    public int AssignedTo { get; set; }
}