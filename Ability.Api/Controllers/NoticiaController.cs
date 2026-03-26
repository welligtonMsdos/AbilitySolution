using Ability.Api.Common;
using Ability.Api.Dtos;
using Ability.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ability.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class NoticiaController : ControllerBase
{
   private readonly INoticiaService _service;

    public NoticiaController(INoticiaService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var noticias = await _service.GetNoticiasAsync();

        return Ok(Result<IEnumerable<NoticiaResponseDto>>.Ok(noticias));
    }
}
