using Ability.Api.Dtos;

namespace Ability.Api.Interfaces;

public interface INoticiaService
{
    Task<ICollection<NoticiaResponseDto>> GetNoticiasAsync();
}
