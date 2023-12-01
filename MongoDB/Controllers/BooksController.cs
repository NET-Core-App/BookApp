using Microsoft.AspNetCore.Mvc;
using MongoDB.Models;
using MongoDB.Services;

namespace MongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService) =>
            _booksService = booksService;

        [HttpGet]
        public async Task<List<Book>> Get(int page, int pageSize) =>
            await _booksService.GetAsync(page, pageSize);

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Book>> Get(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<IActionResult> Post(List<Book> books)
        {
            try
            {
                await _booksService.CreateAsync(books);

                var bookIds = books.Select(item => item.Id).ToList();
                return CreatedAtAction(nameof(Get), new { ids = bookIds }, books);
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log the exception)
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("GenerateBookOnly")]
        public async Task<IActionResult> RandomPost(int count)
        {
            try
            {
                await _booksService.CreateRandomAsync(count);
                return Ok(" created successfully.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Book updatedBook)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            updatedBook.Id = book.Id;

            await _booksService.UpdateAsync(id, updatedBook);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _booksService.RemoveAsync(id);

            return NoContent();
        }
    }
}
