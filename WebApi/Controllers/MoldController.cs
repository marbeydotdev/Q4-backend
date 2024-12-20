using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("molds")]
public class MoldController : ControllerBase
{
    private readonly MoldRepository _moldRepository;

    public MoldController(MoldRepository moldRepository)
    {
        _moldRepository = moldRepository;
    }

    // [HttpGet("{moldId}/history")]
    // public async Task<IActionResult> GetHistory(int? moldId, int skip = 0, int limit = 10)
    // {
    //     object results;
    //     if (moldId.HasValue)
    //     {
    //         Console.WriteLine(moldId.Value);
    //         results = await _moldRepository.GetMoldHistoryAsync(skip, limit, moldId.Value);
    //     }
    //     else
    //     {
    //         results = await _moldRepository.GetMoldHistoryAsync(skip, limit);
    //     }
    //
    //     return Ok(results);
    // }

    [HttpGet("")]
    public async Task<IActionResult> GetList(string? search, int skip = 0, int limit = 10)
    {
        var results = await _moldRepository.GetMolds(skip, limit);
        return Ok(results);
    }

    [HttpGet("{moldId}")]
    public async Task<IActionResult> GetMoldById(int moldId)
    {
        var mold = await _moldRepository.GetMoldByIdAsync(moldId);
        if (mold == null)
        {
            return NotFound();
        }
        return Ok(mold);
    }

    [HttpGet("{moldId}/shothistory")]
    public async Task<IActionResult> GetHistory(int moldId, DateTime lastMaintenance)
    {
        var shots = await _moldRepository.GetMoldShotHistory(moldId, lastMaintenance);
        return Ok(shots);
    }

    [HttpGet("{moldId}/health")]
    public async Task<IActionResult> GetHealth(int moldId, DateTime lastMaintenance, double tolerance = 0.5)
    {
        var health = await _moldRepository.GetMoldHealth(moldId, lastMaintenance, tolerance);
        return Ok(new { health });
    }
}