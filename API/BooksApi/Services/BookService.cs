using BooksApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace BooksApi.Services 
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        public BookService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);          
        }

        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        public Book Get(string Id) => _books.Find<Book>(book => book.Id == Id).FirstOrDefault();

        public Book Create(Book book)
        {
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookUp) =>
            _books.ReplaceOne(book => book.Id == id, bookUp);

        public void Remove(Book bookRm) =>
            _books.DeleteOne(book => book.Id == bookRm.Id);

        public void Remove(string id) =>
            _books.DeleteOne(book => book.Id == id);
    }
}