using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("machine")]
public class MachineController : ControllerBase
{
    private readonly MachineRepository _machineRepository;

    public MachineController(MachineRepository machineRepository)
    {
        _machineRepository = machineRepository;
    }

    [HttpGet("list")]
    public async Task<IActionResult> ListMachines(string search = "", int skip = 0, int limit = 10)
    {
        var machines = await _machineRepository.GetAllAsync(skip, limit);
        return Ok(machines);
    }

    [HttpGet("shots")]
    public async Task<IActionResult> GetMachineShotHistory(int machineId, DateTime from, DateTime to)
    {
        var shotHistory = await _machineRepository.GetMachineShotHistoryAsync(machineId, from, to);
        return Ok(shotHistory);
    }
}