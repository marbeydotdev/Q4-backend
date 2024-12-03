using Common.Enums;

namespace Common.dto;

public class UpdateMaintenanceDto
{
    public int Id { get; set; }
    public DateTime PlannedDate { get; set; }
    public MaintenanceType MaintenanceType { get; set; }
    public MaintenanceStatus MaintenanceStatus { get; set; }
    public string Description { get; set; } = null!;
    public int AssignedTo { get; set; }
}