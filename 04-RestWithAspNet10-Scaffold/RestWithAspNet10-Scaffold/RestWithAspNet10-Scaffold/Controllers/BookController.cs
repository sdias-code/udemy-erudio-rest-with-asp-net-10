using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.Book;
using RestWithAspNet10_Scaffold.Services;


namespace RestWithAspNet10_Scaffold.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService service,
            ILogger<BookController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Fetching all books");

            var books = _service.FindAll();

            if (!books.Any()) return NotFound();

            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            _logger.LogInformation("Fetching book with ID {id}", id);

            var book = _service.FindById(id);

            if (book == null) return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public IActionResult Post([FromBody] BookCreateDTO dto)
        {
            _logger.LogInformation("Creating new Book: {firstName}", dto.Title);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = _service.Create(dto);

            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult Put(long id, [FromBody] BookUpdateDTO dto)
        {

            if(!ModelState.IsValid) return BadRequest(ModelState);

            _logger.LogInformation("Updating book with ID {id}", id);

            dto.Id = id;

            var updated = _service.Update(dto);

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            _logger.LogInformation("Deleting book with ID {id}", id);
            _service.Delete(id);
            _logger.LogDebug("Book with ID {id} deleted successfully", id);
            return NoContent();
        }
    }
}
