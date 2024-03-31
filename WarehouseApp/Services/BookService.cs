using Book1.Interfaces;
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
        public int CountAvailable { get; set; }
    }

    public class BookService : IDatabaseService
    {
        //private readonly string _connectionString = "server=localhost;port=3300;user=root;password=8josd12M;database=books;";
        private readonly string _connectionString;
        public BookService(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<Book>> GetBooksAsync()
        {
            var books = new List<Book>();
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "SELECT Id, BookId, CountAvailable FROM books";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var book = new Book
                        {
                            Id = reader.GetInt32("Id"),
                            BookId = reader.GetInt32("BookId"),
                            CountAvailable = reader.GetInt32("CountAvailable")
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
                var query = "SELECT * FROM books WHERE Id = @id";
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
                                CountAvailable = reader.GetInt32("CountAvailable")
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
                var query = "INSERT INTO books (BookId, CountAvailable) VALUES (@bookId, @countAvailable)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@bookId", book.BookId);
                    command.Parameters.AddWithValue("@countAvailable", book.CountAvailable);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateBookAsync(int id, Book book)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "UPDATE books SET BookId = @bookId, CountAvailable = @countAvailable WHERE Id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@bookId", book.BookId);
                    command.Parameters.AddWithValue("@countAvailable", book.CountAvailable);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task DeleteBookAsync(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "DELETE FROM books WHERE Id = @id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
