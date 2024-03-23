using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WarehouseApp.Services;

namespace WarehouseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookServices _bookService = new BookServices();
        public BooksController()
        {
            _bookService = new BookServices();
        }
        // GET: api/<BooksController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var books = await _bookService.GetBooksAsync();
            return Ok(books);
        }

        // GET api/<BooksController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        // POST api/<BooksController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Book book)
        {
            await _bookService.AddBookAsync(book);
            return Ok();
        }

        // PUT api/<BooksController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Book book)
        {
            var existingBook = await _bookService.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }
            await _bookService.UpdateBookAsync(id, book);
            return Ok();
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existingBook = await _bookService.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }
            await _bookService.DeleteBookAsync(id);
            return Ok();
        }
    }
}
