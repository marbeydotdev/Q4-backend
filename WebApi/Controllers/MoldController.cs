using Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("mold")]
public class MoldController : ControllerBase
{
    private readonly MoldRepository _moldRepository;

    public MoldController(MoldRepository moldRepository)
    {
        _moldRepository = moldRepository;
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(int? moldId, int skip = 0, int limit = 10)
    {
        object results;
        if (moldId.HasValue)
        {
            Console.WriteLine(moldId.Value);
            results = await _moldRepository.GetMoldHistoryAsync(skip, limit, moldId.Value);
        }
        else
        {
            results = await _moldRepository.GetMoldHistoryAsync(skip, limit);
        }

        return Ok(results);
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetList(string? search, int skip = 0, int limit = 10)
    {
        var results = await _moldRepository.GetMolds(skip, limit);
        return Ok(results);
    }
}