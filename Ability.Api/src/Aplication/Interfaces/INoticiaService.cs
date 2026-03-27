using Ability.Api.src.Aplication.Dtos;

namespace Ability.Api.src.Aplication.Interfaces;

public interface INoticiaService
{
    Task<ICollection<NoticiaResponseDto>> GetNoticiasAsync();

    Task<NoticiaResponseDto> GetNoticiaById(string id);

    Task<NoticiaResponseDto> CreateNoticiaAsync(NoticiaComandDto noticiaComandDto);

    Task<NoticiaResponseDto> AtualizarNoticiaAsync(string id, NoticiaComandDto noticiaComandDto);

    Task<bool> DeletarNoticiaAsync(string id);
}
