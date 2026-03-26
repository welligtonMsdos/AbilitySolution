using Ability.Api.Dtos;
using Ability.Domain.Entities;

namespace Ability.Api.Extensions;

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
}
