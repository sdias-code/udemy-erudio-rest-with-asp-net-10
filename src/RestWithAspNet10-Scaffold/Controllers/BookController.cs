using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.DTOs.V1.Book;
using RestWithAspNet10_Scaffold.Hypermedia;
using RestWithAspNet10_Scaffold.Services;

namespace RestWithAspNet10_Scaffold.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _service;
        private readonly ILogger<BookController> _logger;

        public BookController(
            IBookService service,
            ILogger<BookController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ==========================
        // GET PAGINADO + FILTROS
        // ==========================
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<BookResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<BookResponseDTO>>> Get(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "id",
            [FromQuery] string direction = "asc",
            [FromQuery] string? search = null,
            [FromQuery] DateTime? launchFrom = null,
            [FromQuery] DateTime? launchTo = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            _logger.LogInformation("Fetching all books");

            var pagedResponse = await _service.FindAllAsync(
                page,
                pageSize,
                sortBy,
                direction,
                search,
                launchFrom,
                launchTo,
                minPrice,
                maxPrice);

            if (!pagedResponse.Items.Any())
                return NotFound();

            // Inicializa links da página
            pagedResponse.Links ??= new List<HypermediaLink>();
            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/v1/book";

            // Links de paginação HATEOAS
            pagedResponse.Links.Add(new HypermediaLink("self", $"{baseUrl}?page={page}&pageSize={pageSize}", "GET"));
            if (page < pagedResponse.TotalPages)
                pagedResponse.Links.Add(new HypermediaLink("next", $"{baseUrl}?page={page + 1}&pageSize={pageSize}", "GET"));
            if (page > 1)
                pagedResponse.Links.Add(new HypermediaLink("prev", $"{baseUrl}?page={page - 1}&pageSize={pageSize}", "GET"));

            // Links HATEOAS para cada item
            foreach (var book in pagedResponse.Items)
            {
                book.Links ??= new List<HypermediaLink>();

                book.Links.Add(new HypermediaLink("self", $"{baseUrl}/{book.Id}", "GET"));
                book.Links.Add(new HypermediaLink("update", $"{baseUrl}/{book.Id}", "PUT"));
                book.Links.Add(new HypermediaLink("delete", $"{baseUrl}/{book.Id}", "DELETE"));
                book.Links.Add(new HypermediaLink("create", $"{baseUrl}", "POST"));
                book.Links.Add(new HypermediaLink("collection", $"{baseUrl}", "GET"));
            }

            return Ok(pagedResponse);
        }

        // ==========================
        // GET BY ID
        // ==========================
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(BookResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BookResponseDTO>> Get(long id)
        {
            _logger.LogInformation("Fetching book with ID {id}", id);

            var book = await _service.FindByIdAsync(id);

            if (book == null)
                return NotFound();

            // Inicializa links HATEOAS para o item
            book.Links ??= new List<HypermediaLink>();
            var baseUrl = $"{Request.Scheme}://{Request.Host}/api/v1/book";

            book.Links.Add(new HypermediaLink("self", $"{baseUrl}/{book.Id}", "GET"));
            book.Links.Add(new HypermediaLink("update", $"{baseUrl}/{book.Id}", "PUT"));
            book.Links.Add(new HypermediaLink("delete", $"{baseUrl}/{book.Id}", "DELETE"));
            book.Links.Add(new HypermediaLink("create", $"{baseUrl}", "POST"));
            book.Links.Add(new HypermediaLink("collection", $"{baseUrl}", "GET"));

            return Ok(book);
        }

        // ==========================
        // CREATE
        // ==========================
        [HttpPost]
        [ProducesResponseType(typeof(BookResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post([FromBody] BookCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Creating new Book: {title}", dto.Title);

            var created = await _service.CreateAsync(dto);

            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        // ==========================
        // UPDATE
        // ==========================
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(BookResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(long id, [FromBody] BookUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("ID da rota diferente do body.");

            var existing = await _service.FindByIdAsync(id);
            if (existing == null)
                return NotFound();

            _logger.LogInformation("Updating book with ID {id}", id);

            var updated = await _service.UpdateAsync(dto);

            return Ok(updated);
        }

        // ==========================
        // DELETE
        // ==========================
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(long id)
        {
            var existing = await _service.FindByIdAsync(id);

            if (existing == null)
                return NotFound();

            _logger.LogInformation("Deleting book with ID {id}", id);

            await _service.DeleteAsync(id);

            return NoContent();
        }
    }
}
