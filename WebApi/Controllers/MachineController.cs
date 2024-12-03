using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("machines")]
public class MachineController : ControllerBase
{
    private readonly MachineRepository _machineRepository;

    public MachineController(MachineRepository machineRepository)
    {
        _machineRepository = machineRepository;
    }

    [HttpGet("")]
    public async Task<IActionResult> ListMachines(string search = "", int skip = 0, int limit = 10)
    {
        var machines = await _machineRepository.GetAllAsync(skip, limit);
        return Ok(machines);
    }
    
    
    [HttpGet("{machineId}/history")]
    public async Task<IActionResult> GetHistory(int machineId, DateTime? from, DateTime? to)
    {
        from ??= DateTime.Now.AddDays(-7);
        to ??= DateTime.Now;
        
        var results = await _machineRepository.GetMachineShotHistoryAsync(machineId, from.Value, to.Value);
    
        return Ok(results);
    }

    [HttpGet("{machineId}/shots")]
    public async Task<IActionResult> GetMachineShotHistory(int machineId, DateTime from, DateTime to)
    {
        var shotHistory = await _machineRepository.GetMachineShotHistoryAsync(machineId, from, to);
        return Ok(shotHistory);
    }
}