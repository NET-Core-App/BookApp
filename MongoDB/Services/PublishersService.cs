using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace MongoDB.Services
{
    public class PublishersService
    {
        private readonly IMongoCollection<Author> _authorCollection;
        private readonly IMongoCollection<AuthorPublisherLink> _authorPublisherLink;
        private readonly IMongoCollection<Publisher> _publisherCollection;

        public PublishersService(
            IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _authorCollection = mongoDatabase.GetCollection<Author>(
                bookStoreDatabaseSettings.Value.AuthorsCollectionName);

            _authorPublisherLink = mongoDatabase.GetCollection<AuthorPublisherLink>(
              bookStoreDatabaseSettings.Value.AuthorPublisherLink);

            _publisherCollection = mongoDatabase.GetCollection<Publisher>(
            bookStoreDatabaseSettings.Value.PublishersCollectionName);
        }

        public async Task CreateAuthorPublisherLinks(List<Publisher> publishers)
        {
            await _publisherCollection.InsertManyAsync(publishers);

            foreach (var publisher in publishers)
            {
                foreach (var authorId in publisher.AuthorIds)
                {
                    var authorPublisherLink = new AuthorPublisherLink
                    {
                        AuthorId = authorId,
                        PublisherId = publisher.Id
                    };

                    await _authorPublisherLink.InsertOneAsync(authorPublisherLink);

                    // Mendapatkan ID baru yang disisipkan
                    var insertedLinkId = authorPublisherLink.Id;

                    //Update Author untuk menambahkan PublisherId
                    var authorFilter = Builders<Author>.Filter.Eq(a => a.Id, authorId);
                    var update = Builders<Author>.Update.Push(a => a.PublisherIds, insertedLinkId);
                    await _authorCollection.UpdateOneAsync(authorFilter, update);
                }
            }
        }


        public async Task CreateAsync(List<Publisher> publishers)
        {
            await CreateAuthorPublisherLinks(publishers);
        }
    }
}
