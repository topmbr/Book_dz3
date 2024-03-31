using WarehouseApp.Services;

namespace Book1.Interfaces
{
    public interface IDatabaseService
    {
        public Task<List<Book>> GetBooksAsync();
        public Task<Book> GetBookByIdAsync(int id);
        public Task AddBookAsync(Book book);
        public Task UpdateBookAsync(int id, Book book);
        public Task DeleteBookAsync(int id);
    }
}
