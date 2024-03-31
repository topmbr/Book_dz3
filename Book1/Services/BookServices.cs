using Book1.Interfaces;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
namespace WarehouseApp.Services
{
    public class Book
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string Author { get; set; }
        public string BookName { get; set; }
        public string Genre { get; set; }
    }

    public class BookServices : IDatabaseService
    {
        //private readonly string _connectionString = "server=localhost;port=3300;user=root;password=8josd12M;database=book;";
        private readonly string _connectionString;

        public BookServices(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var books = new List<Book>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT Id, BookId, Author, BookName, Genre FROM Book";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var book = new Book
                        {
                            Id = reader.GetInt32("Id"),
                            BookId = reader.GetInt32("BookId"),
                            Author = reader.GetString("Author"),
                            BookName = reader.GetString("BookName"),
                            Genre = reader.GetString("Genre")
                        };
                        books.Add(book);
                    }
                }
            }
            return books;
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT * FROM Book WHERE Id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Book
                            {
                                Id = reader.GetInt32("Id"),
                                BookId = reader.GetInt32("BookId"),
                                Author = reader.GetString("Author"),
                                BookName = reader.GetString("BookName"),
                                Genre = reader.GetString("Genre")
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }

        public async Task AddBookAsync(Book book)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "INSERT INTO Book (BookId, Author, BookName, Genre) VALUES (@bookId, @author, @bookName, @genre)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bookId", book.BookId);
                    command.Parameters.AddWithValue("@author", book.Author);
                    command.Parameters.AddWithValue("@bookName", book.BookName);
                    command.Parameters.AddWithValue("@genre", book.Genre);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateBookAsync(int id, Book book)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "UPDATE Book SET BookId = @bookId, Author = @author, BookName = @bookName, Genre = @genre WHERE Id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@bookId", book.BookId);
                    command.Parameters.AddWithValue("@author", book.Author);
                    command.Parameters.AddWithValue("@bookName", book.BookName);
                    command.Parameters.AddWithValue("@genre", book.Genre);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "DELETE FROM Book WHERE Id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
