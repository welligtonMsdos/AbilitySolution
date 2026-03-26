using Ability.Domain.Entities;
using Ability.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ability.Infrastructure.Repositories;

public class NoticiaRepository : INoticiaRepository
{
    private readonly IMongoCollection<Noticia> _mongoCollection;

    public NoticiaRepository(IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase("RpaMsn");

        _mongoCollection = database.GetCollection<Noticia>("Noticias");
    }

    public async Task<ICollection<Noticia>> GetNoticiasAsync()
    {
        var news = await _mongoCollection.Find(new BsonDocument()).ToListAsync();

        return news;
    }
}
 