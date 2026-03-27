using Ability.Api.src.Aplication.Dtos;
using Ability.Api.src.Aplication.Extensions;
using Ability.Api.src.Aplication.Interfaces;
using Ability.Api.src.Aplication.Validators;
using Ability.Domain.Interfaces;
using FluentValidation;

namespace Ability.Api.src.Aplication.Services;

public class NoticiaService : INoticiaService
{
    private readonly INoticiaRepository _repository;
    private readonly NoticiaValidator _validator;

    public NoticiaService(INoticiaRepository repository,
                          NoticiaValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<NoticiaResponseDto> AtualizarNoticiaAsync(string id, NoticiaComandDto noticiaComandDto)
    {
        await _validator.ValidateAndThrowAsync(noticiaComandDto);

        ArgumentNullException.ThrowIfNull(id);

        var noticia = noticiaComandDto.ToEntity();        

        var updatedNoticia = await _repository.AtualizarNoticiaAsync(id, noticia);

        return updatedNoticia.ToNoticiaDto();
    }

    public async Task<NoticiaResponseDto> CreateNoticiaAsync(NoticiaComandDto noticiaComandDto)
    {
        await _validator.ValidateAndThrowAsync(noticiaComandDto);

        var noticia = noticiaComandDto.ToEntity();

        var createdNoticia = await _repository.CreateNoticiaAsync(noticia);

        return createdNoticia.ToNoticiaDto();
    }

    public async Task<bool> DeletarNoticiaAsync(string id)
    {
        ArgumentNullException.ThrowIfNull(id); 

        return await _repository.DeletarNoticiaAsync(id);
    }

    public async Task<NoticiaResponseDto> GetNoticiaById(string id)
    {
        var noticia = await _repository.GetNoticiaById(id);

        ArgumentNullException.ThrowIfNull(noticia);

        return noticia.ToNoticiaDto();
    }

    public async Task<ICollection<NoticiaResponseDto>> GetNoticiasAsync()
    {
        var news = await _repository.GetNoticiasAsync();

        ArgumentNullException.ThrowIfNull(news);

        return news.Select(e => e.ToNoticiaDto()).ToList();
    }
}
