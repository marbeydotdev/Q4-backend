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
    public async Task<IActionResult> ListMachines()
    {
        var machines = await _machineRepository.GetAllAsync();
        return Ok(machines);
    }

    [HttpGet("shot_history")]
    public async Task<IActionResult> GetMachineShotHistory(int board, int port, int skip = 0, int limit = 10)
    {
        var shotHistory = await _machineRepository.GetMachineShotHistoryAsync(board, port, skip, limit);
        return Ok(shotHistory);
    }
}