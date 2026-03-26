using Ability.Domain.Entities;

namespace Ability.Domain.Interfaces;

public interface IRpaRepository
{
    Task SalvarNoticiaAsync(Noticia noticia);
    Task<bool> JaExisteUrlAsync(string url);
}

