using Ability.Api.src.Aplication.Common;
using Ability.Api.src.Aplication.Dtos;
using Ability.Api.src.Aplication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ability.Api.src.Aplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NoticiaController : ControllerBase
{
   private readonly INoticiaService _service;

    public NoticiaController(INoticiaService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] NoticiaComandDto noticiaDto)
    {
        var result = await _service.CreateNoticiaAsync(noticiaDto);

        return CreatedAtAction(nameof(GetById),
               new { id = result.Id },
               Result<NoticiaResponseDto>.Ok(result, "Notícia criada com sucesso!"));
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var noticias = await _service.GetNoticiasAsync();

        return Ok(Result<IEnumerable<NoticiaResponseDto>>.Ok(noticias));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var noticia = await _service.GetNoticiaById(id);

        if (noticia is null)
            return NotFound(Result<object>.Failure("Notícia não encontrada!"));

        return Ok(Result<NoticiaResponseDto>.Ok(noticia));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, [FromBody] NoticiaComandDto noticiaDto)
    {
        var noticia = await _service.AtualizarNoticiaAsync(id, noticiaDto);

        if (noticia is null)
            return NotFound(Result<object>.Failure("Notícia não encontrada para atualização."));

        return Ok(Result<NoticiaResponseDto>.Ok(noticia, "Notícia atualizada com sucesso!"));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var noticia = await _service.DeletarNoticiaAsync(id);

        if (!noticia)
            return NotFound(Result<object>.Failure("Notícia não encontrada para deletar."));

        return Ok(Result<bool>.Ok(true, "Notícia deletada com sucesso!"));
    }
}
