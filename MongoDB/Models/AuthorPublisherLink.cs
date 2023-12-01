using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDB.Models
{
    public class AuthorPublisherLink
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string AuthorId { get; set; }
        public string PublisherId { get; set; }
    }
}
