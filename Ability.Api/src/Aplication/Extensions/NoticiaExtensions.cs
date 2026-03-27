using Ability.Api.src.Aplication.Dtos;
using Ability.Domain.Entities;

namespace Ability.Api.src.Aplication.Extensions;

public static class NoticiaExtensions
{
    public static NoticiaResponseDto ToNoticiaDto(this Noticia noticia)
    {
        ArgumentNullException.ThrowIfNull(noticia);

        ArgumentNullException.ThrowIfNull(noticia.Id);

        return new NoticiaResponseDto(noticia.Id,
                                      noticia.Titulo,
                                      noticia.Url,
                                      noticia.DataCapturada);
    }

    public static Noticia ToEntity(this NoticiaComandDto noticiaDto)
    {
        ArgumentNullException.ThrowIfNull(noticiaDto);

        return new Noticia
        {
            Titulo = noticiaDto.Titulo,
            Url = noticiaDto.Url
        };
    }
}
