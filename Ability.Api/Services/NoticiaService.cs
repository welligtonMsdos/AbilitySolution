using Ability.Api.Dtos;
using Ability.Api.Extensions;
using Ability.Api.Interfaces;
using Ability.Domain.Interfaces;

namespace Ability.Api.Services;

public class NoticiaService : INoticiaService
{
    private readonly INoticiaRepository _repository;

    public NoticiaService(INoticiaRepository repository)
    {
        _repository = repository;
    }

    public async Task<ICollection<NoticiaResponseDto>> GetNoticiasAsync()
    {
        var news = await _repository.GetNoticiasAsync();

        ArgumentNullException.ThrowIfNull(news);

        return news.Select(e => e.ToNoticiaDto()).ToList();
    }
}
