using Ability.Api.Dtos;

namespace Ability.Api.src.Aplication.Interfaces;

public interface INoticiaService
{
    Task<ICollection<NoticiaResponseDto>> GetNoticiasAsync();
}
