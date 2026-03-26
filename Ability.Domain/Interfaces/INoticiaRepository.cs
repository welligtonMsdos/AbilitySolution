using Ability.Domain.Entities;

namespace Ability.Domain.Interfaces;

public interface INoticiaRepository
{
    Task<ICollection<Noticia>> GetNoticiasAsync();
}
