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
    public async Task<IActionResult> GetHistory(int skip = 0, int limit = 10)
    {
        var results = await _moldRepository.GetMoldHistoryAsync(skip, limit);
        return Ok(results); 
    }
}