using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Models;

namespace MongoDB.Services
{
    public class AuthorsService
    {
        private readonly IMongoCollection<Author> _authorCollection;
        public AuthorsService(
            IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
           bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _authorCollection = mongoDatabase.GetCollection<Author>(
            bookStoreDatabaseSettings.Value.AuthorsCollectionName);
        }

        public async Task<List<Author>> GetAuthors() =>
              await _authorCollection.Find(author => true).ToListAsync();

    }
}
