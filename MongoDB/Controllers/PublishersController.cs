using Microsoft.AspNetCore.Mvc;
using MongoDB.Models;
using MongoDB.Services;

namespace MongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublishersController : ControllerBase
    {
        private readonly PublishersService _publishersService;

        public PublishersController(PublishersService publishersService) =>
            _publishersService = publishersService;

        [HttpPost]
        public async Task<IActionResult> Post(List<Publisher> publishers)
        {
            try
            {
                await _publishersService.CreateAsync(publishers);

                return Ok("Many-to-Many relationship created successfully.");
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log the exception)
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
