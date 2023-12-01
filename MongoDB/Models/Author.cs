using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace MongoDB.Models
{
    public class Author
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string AuthorName { get; set; } = null!;
        public List<string> BookIds { get; set; } = new List<string>();
        public List<string> PublisherIds { get; set; } = new List<string>();
    }
}
