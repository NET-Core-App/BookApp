using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace MongoDB.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
     /*   [JsonIgnore]*/
        public string? Id { get; set; }

        [BsonElement("Name")]
        [JsonPropertyName("Name")]
        public string BookName { get; set; } = null!;

        public decimal Price { get; set; }

        public string Category { get; set; } = null!;

        public string Author { get; set; } = null!;
        public int Pages { get; set; }

        public DateTime PublicationDate { get; set; }

        public bool IsAvailable { get; set; }

        public string Publisher { get; set; } = null!;

        public string ISBN { get; set; } = null!;

        public string Language { get; set; } = null!;

        public string Edition { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public string Format { get; set; } = null!;

        public string Country { get; set; } = null!;

        public string Translator { get; set; } = null!;

        public string Series { get; set; } = null!;

        public string Awards { get; set; } = null!;

        public double AverageRating { get; set; }

        public int NumberOfReviews { get; set; }

        public bool IsBestseller { get; set; }

        public string CoverImageUrl { get; set; } = null!;

        public string[] Tags { get; set; } = Array.Empty<string>();

        public string[] Characters { get; set; } = Array.Empty<string>();

        public string MainTheme { get; set; } = null!;

        public string Setting { get; set; } = null!;

        public string[] Quotes { get; set; } = Array.Empty<string>();

        public string AlternateTitle { get; set; } = null!;

        public string Prequel { get; set; } = null!;

        public string Sequel { get; set; } = null!;

        public string RelatedBooks { get; set; } = null!;

        public string LibraryOfCongressNumber { get; set; } = null!;

        public string DeweyDecimal { get; set; } = null!;

        public string FormatDetails { get; set; } = null!;
    }
}
