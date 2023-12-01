namespace MongoDB.Models
{
    public class BookStoreDatabaseSettings
    {
        public string ConnectionString { get; set; } = null!;

        public string DatabaseName { get; set; } = null!;

        public string BooksCollectionName { get; set; } = null!;

        public string AuthorsCollectionName { get; set; } = null!;

        public string PublishersCollectionName { get; set; } = null!;

        public string AuthorPublisherLink { get; set; } = null!;

    }
}
