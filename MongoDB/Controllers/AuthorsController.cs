using Microsoft.AspNetCore.Mvc;
using MongoDB.Models;
using MongoDB.Services;

namespace MongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly AuthorsService _authorsService;

        public AuthorsController(AuthorsService authorsService) =>
            _authorsService = authorsService;

        [HttpGet]
        public async Task<List<Author>> Get() =>
          await _authorsService.GetAuthors();

    }
}
