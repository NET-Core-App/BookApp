using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Models;
using System;
using static System.Reflection.Metadata.BlobBuilder;

namespace MongoDB.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;
        private readonly IMongoCollection<Author> _authorCollection;

        public BooksService(
            IOptions<BookStoreDatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
                bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<Book>(
                bookStoreDatabaseSettings.Value.BooksCollectionName);

            _authorCollection = mongoDatabase.GetCollection<Author>(
                bookStoreDatabaseSettings.Value.AuthorsCollectionName);
        }

        public async Task<List<Book>> GetAsync(int page, int pageSize)
        {
            // Hitung skip berdasarkan halaman dan ukuran halaman
            int skip = (page - 1) * pageSize;

            // Lakukan query ke MongoDB dengan menggunakan Skip dan Limit
            var books = await _booksCollection
                .Find(_ => true)
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();

            return books;
        }

        public async Task<Book?> GetAsync(string id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        //crate book dan author tanpa bulk
        public async Task CreateAsync(List<Book> books)
        {
            await _booksCollection.InsertManyAsync(books);

            foreach (var book in books)
            {
                var authorFilter = Builders<Author>.Filter.Eq(a => a.AuthorName, book.Author);
                var existingAuthor = _authorCollection.Find(authorFilter).FirstOrDefault();

                if (existingAuthor == null)
                {
                    var newAuthor = new Author { AuthorName = book.Author };
                    newAuthor.BookIds.Add(book.Id);
                    _authorCollection.InsertOne(newAuthor);
                }
                else
                {
                    existingAuthor.BookIds.Add(book.Id);
                    var update = Builders<Author>.Update.Set(a => a.BookIds, existingAuthor.BookIds);
                    _authorCollection.UpdateOne(authorFilter, update);
                }
            }
        }

        //dengan bulk, create book only tanpa author
        public async Task CreateRandomAsync(int count)
        {
            Random random = new Random();
            var bulkOps = new List<WriteModel<Book>>();
            var bulkAuthor = new List<WriteModel<Author>>();

            for (int i = 0; i < count; i++)
            {
                Book book = new Book
                {
                    BookName = $"Book{i + 1}",
                    Price = (decimal)(random.NextDouble() * 100),
                    Category = $"Category{i % 5 + 1}",
                    Author = $"Author{i % 3 + 1}",
                    Pages = random.Next(100, 10000000),
                    PublicationDate = DateTime.Now.AddDays(-random.Next(1, 365)),
                    IsAvailable = random.Next(0, 2) == 0 ? false : true,
                    Publisher = $"Publisher{i % 2 + 1}",
                    ISBN = $"{random.Next(1000, 9999)}-{random.Next(10000, 99999)}-{random.Next(100, 999)}-{random.Next(1, 9)}",
                    Language = $"Language{i % 4 + 1}",
                    Edition = $"Edition{i % 3 + 1}",
                    Description = $"Description for Book {i + 1}",
                    Genre = $"Genre{i % 6 + 1}",
                    Format = $"Format{i % 2 + 1}",
                    Country = $"Country{i % 3 + 1}",
                    Translator = $"Translator{i % 2 + 1}",
                    Series = $"Series{i % 3 + 1}",
                    AverageRating = random.NextDouble() * 5,
                    NumberOfReviews = random.Next(1, 100),
                    IsBestseller = random.Next(0, 2) == 0 ? false : true,
                    CoverImageUrl = $"https://example.com/cover/{i + 1}.jpg",
                    Tags = Enumerable.Range(1, random.Next(1, 5)).Select(_ => $"Tag{_}").ToArray(),
                    Characters = Enumerable.Range(1, random.Next(1, 3)).Select(_ => $"Character{_}").ToArray(),
                    MainTheme = $"Main Theme{i % 4 + 1}",
                    Setting = $"Setting{i % 3 + 1}",
                    Quotes = Enumerable.Range(1, random.Next(1, 3)).Select(_ => $"Quote{_}").ToArray(),
                    AlternateTitle = $"Alternate Title{i + 1}",
                    Prequel = $"Prequel{i % 2 + 1}",
                    Sequel = $"Sequel{i % 2 + 1}",
                    RelatedBooks = $"Related Books{i % 5 + 1}",
                    LibraryOfCongressNumber = $"LCCN{i + 1}",
                    DeweyDecimal = $"DDC{i % 2 + 1}",
                    FormatDetails = $"Format Details{i % 3 + 1}"
                };

                bulkOps.Add(new InsertOneModel<Book>(book));
            }

            await Task.WhenAll(
                _booksCollection.BulkWriteAsync(bulkOps)
            );
        }

        public async Task UpdateAsync(string id, Book updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}
