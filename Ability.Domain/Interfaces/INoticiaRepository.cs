using Ability.Domain.Entities;

namespace Ability.Domain.Interfaces;

public interface INoticiaRepository
{
    Task<ICollection<Noticia>> GetNoticiasAsync();

    Task<Noticia> GetNoticiaById(string id);

    Task<Noticia> CreateNoticiaAsync(Noticia noticia);

    Task<Noticia> AtualizarNoticiaAsync(string id, Noticia noticia);

    Task<bool> DeletarNoticiaAsync(string id);
}
