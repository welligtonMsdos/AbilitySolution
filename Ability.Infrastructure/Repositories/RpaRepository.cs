using Ability.Domain.Entities;
using Ability.Domain.Interfaces;
using MongoDB.Driver;

namespace Ability.Infrastructure.Repositories;

public class RpaRepository : IRpaRepository
{
    private readonly IMongoCollection<Noticia> _mongoCollection;

    public RpaRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("RpaMsn");

        _mongoCollection = database.GetCollection<Noticia>("Noticias");
    }

    public async Task<bool> JaExisteUrlAsync(string url)
    {
        return await _mongoCollection.Find(x => x.Url == url).AnyAsync();
    }

    public async Task SalvarNoticiaAsync(Noticia noticia)
    {
        await _mongoCollection.InsertOneAsync(noticia);
    }
}
