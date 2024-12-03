using Common.dto;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("maintenances")]
public class MaintenanceController : ControllerBase
{
    private readonly MaintenanceRepository _maintenance;

    public MaintenanceController(MaintenanceRepository maintenance)
    {
        _maintenance = maintenance;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetMaintenanceList()
    {
        var maintenance = await _maintenance.GetPlannedMaintenance();
        return Ok(maintenance);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetMaintenanceList(int id)
    {
        var maintenance = await _maintenance.GetMaintenance(id);
        return maintenance.IsSuccess ? Ok(maintenance.Value) : BadRequest(maintenance.Errors);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateMaintenance([FromBody] CreateMaintenanceDto createMaintenanceDto)
    {
        var maintenance = await _maintenance.CreateMaintenance(createMaintenanceDto);

        return maintenance.IsSuccess ? Ok(maintenance.Value) : BadRequest(maintenance.Errors);
    }

    [HttpPatch("")]
    public async Task<IActionResult> UpdateMaintenance([FromBody] UpdateMaintenanceDto updateMaintenanceDto)
    {
        var maintenance = await _maintenance.UpdateMaintenance(updateMaintenanceDto);
        return maintenance.IsSuccess ? Ok() : BadRequest(maintenance.Errors);
    }

    [HttpDelete("")]
    public async Task<IActionResult> DeleteMaintenance(int id)
    {
        var delete = await _maintenance.DeleteMaintenance(id);

        return delete.IsSuccess ? Ok() : BadRequest(delete.Errors);
    }
}