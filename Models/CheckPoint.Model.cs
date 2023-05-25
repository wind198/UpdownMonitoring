using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UpdownMonitoring.Models;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [BsonElement("id")]
    public string Id { get; set; } = null!;

    [BsonElement("isDown")]
    public bool? IsDown { get; set; }

    [BsonElement("responseTime")]
    public int ResponseTime { get; set; }

    [BsonElement("outageAnaysis")]
    public object? OutageAnaysis { get; set; }
}
