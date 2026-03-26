using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ability.Domain.Entities;

public class Noticia
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime DataCapturada { get; set; } = DateTime.UtcNow;
}
