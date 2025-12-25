using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.Model;
using RestWithAspNet10_Scaffold.Services;
using RestWithAspNet10_Scaffold.Services.Implementations;

namespace RestWithAspNet10_Scaffold.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IGenericService<Book> _service;
        private readonly ILogger<BookController> _logger;

        public BookController(IGenericService<Book> service,
            ILogger<BookController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Fetching all books");
            return Ok(_service.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            _logger.LogInformation("Fetching book with ID {id}", id);

            var book = _service.FindById(id);
          
            return Ok(book);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Book book)
        {
            _logger.LogInformation("Creating new Book: {firstName}", book.Title);

            var createdBook = _service.Create(book);

            if (createdBook == null)
            {
                _logger.LogError("Failed to create book with name {firstName}", book.Title);

                return NotFound();
            }
            return Ok(createdBook);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Book book)
        {
            _logger.LogInformation("Updating book with ID {id}", book.Id);

            var createdBook = _service.Update(book);

            if (createdBook == null)
            {
                _logger.LogError("Failed to update book with ID {id}", book.Id);
                return NotFound();
            }
            _logger.LogDebug("Book updated successfully: {firstName}", createdBook.Title);

            return Ok(createdBook);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Deleting book with ID {id}", id);
            _service.Delete(id);
            _logger.LogDebug("Book with ID {id} deleted successfully", id);
            return NoContent();
        }
    }
}
