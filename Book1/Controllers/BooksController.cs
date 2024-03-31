using Book1.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;
using System.Net.Http.Json;
using WarehouseApp.Services;
using static System.Reflection.Metadata.BlobBuilder;

namespace WarehouseApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        //private readonly BookServices _bookService = new BookServices();
        private readonly IDatabaseService _bookService;
        private readonly HttpClient _httpClient;

        public BooksController(IDatabaseService bookService, HttpClient httpClient)
        {
            _bookService = bookService;
            _httpClient = httpClient;
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

        [HttpGet("inventory/{bookId}")]
        public async Task<IActionResult> GetBookInventory(int bookId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"http://localhost:7056/api/Books/inventory/{bookId}");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                var count = await response.Content.ReadFromJsonAsync<Book>();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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
            try
            {
                await _bookService.UpdateBookAsync(id, book);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE api/<BooksController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _bookService.GetBookByIdAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
