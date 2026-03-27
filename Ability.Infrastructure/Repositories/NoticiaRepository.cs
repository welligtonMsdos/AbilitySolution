using Ability.Domain.Entities;
using Ability.Domain.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Ability.Infrastructure.Repositories;

public class NoticiaRepository : INoticiaRepository
{
    private readonly IMongoCollection<Noticia> _mongoCollection;

    public NoticiaRepository(IMongoClient mongoClient, string dbName)
    {
        var database = mongoClient.GetDatabase("RpaMsn");

        _mongoCollection = database.GetCollection<Noticia>("Noticias");
    }

    public async Task<Noticia> AtualizarNoticiaAsync(string id, Noticia noticia)
    {
        noticia.Id = id;
       
        var result = await _mongoCollection.ReplaceOneAsync(n => n.Id == id, noticia);

        return noticia;
    }

    public async Task<Noticia> CreateNoticiaAsync(Noticia noticia)
    {
        await _mongoCollection.InsertOneAsync(noticia);

        return noticia;
    }

    public async Task<bool> DeletarNoticiaAsync(string id)
    {
        var result = await _mongoCollection.DeleteOneAsync(n => n.Id == id);
        
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<Noticia> GetNoticiaById(string id)
    {
        return await _mongoCollection
                        .Find(n => n.Id == id)
                        .FirstOrDefaultAsync();
    }

    public async Task<ICollection<Noticia>> GetNoticiasAsync()
    {
        var news = await _mongoCollection.Find(new BsonDocument()).ToListAsync();

        return news;
    }
}
 