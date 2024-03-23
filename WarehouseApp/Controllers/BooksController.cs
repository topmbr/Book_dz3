using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseApp.Services;

namespace WarehouseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService _bookService = new BookService();
        public BooksController()
        {
            _bookService = new BookService();
        }
        // GET api/BookInventory
        [HttpGet]
        public async Task<IActionResult> GetBookInventory()
        {
            try
            {
                var bookInventory = await _bookService.GetBooksAsync();
                return Ok(bookInventory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET api/BookInventory/{bookId}
        [HttpGet("{bookId}")]
        public async Task<IActionResult> GetBookInventory(int bookId)
        {
            try
            {
                var count = await _bookService.GetBookByIdAsync(bookId);
                if (count == null)
                    return NotFound();
                
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST api/BookInventory
        [HttpPost]
        public async Task<IActionResult> AddBookInventory([FromBody] Book book)
        {
            try
            {
                await _bookService.AddBookAsync(book);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT api/BookInventory/{bookId}
        [HttpPut("{bookId}")]
        public async Task<IActionResult> UpdateBookInventory(int bookId, [FromBody] Book book)
        {
            try
            {
                await _bookService.UpdateBookAsync(bookId, book);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/BookInventory/{bookId}
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> DeleteBookInventory(int bookId)
        {
            try
            {
                await _bookService.DeleteBookAsync(bookId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
