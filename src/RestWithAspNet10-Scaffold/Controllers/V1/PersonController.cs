using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.DTOs.V1;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using RestWithAspNet10_Scaffold.Files.Exporters.Contract.Factory;
using RestWithAspNet10_Scaffold.Services;

namespace RestWithAspNet10_Scaffold.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _service;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonService service, ILogger<PersonController> logger)
        {
            _service = service;
            _logger = logger;
        }

        // ============================
        // GET BY ID
        // ============================
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(PersonResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonResponseDTO>> Get(long id)
        {
            var person = await _service.FindByIdAsync(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        // ============================
        // GET PAGINADO
        // ============================
        [HttpGet]
        [Produces("application/json", "application/xml")]
        [ProducesResponseType(typeof(PagedResponse<PersonResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<PersonResponseDTO>>> Get(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "id",
            [FromQuery] string direction = "asc",
            [FromQuery] string? search = null)
        {
            var result = await _service.FindAllAsync(
                page,
                pageSize,
                sortBy,
                direction,
                search);

            if (!result.Items.Any())
                return NotFound();

            _logger.LogInformation("Listando pessoas.");

            return Ok(result);
        }

        // ============================
        // ENABLE
        // ============================
        [HttpPatch("{id:long}/enable")]
        [ProducesResponseType(typeof(PersonResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonResponseDTO>> Enable(long id)
        {
            var person = await _service.Enable(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        // ============================
        // DISABLE
        // ============================
        [HttpPatch("{id:long}/disable")]
        [ProducesResponseType(typeof(PersonResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PersonResponseDTO>> Disable(long id)
        {
            var person = await _service.Disable(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        // ============================
        // CREATE
        // ============================
        [HttpPost]
        [ProducesResponseType(typeof(PersonResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] PersonCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPerson = await _service.CreateAsync(dto);

            return CreatedAtAction(
                nameof(Get),
                new { id = createdPerson.Id },
                createdPerson);
        }

        // ============================
        // UPDATE
        // ============================
        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(PersonResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Put(long id, [FromBody] PersonUpdateDTO dto)
        {
            if (id != dto.Id)
                return BadRequest("ID da rota diferente do body.");

            var updatedPerson = await _service.UpdateAsync(id, dto);

            if (updatedPerson == null)
                return NotFound();

            return Ok(updatedPerson);
        }

        // ============================
        // DELETE
        // ============================
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(List<PersonResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Import([FromForm] FileUploadDTO request)
        {
            var file = request.File;

            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("Tentativa de importação sem arquivo.");
                return BadRequest("Arquivo não fornecido.");
            }

            var importedPersons = await _service.ImportFromFileAsync(file);

            _logger.LogInformation(
                "Importação concluída. Total importado: {Count}",
                importedPersons.Count);

            return Ok(importedPersons);
        }

        [HttpGet("export")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces(MediaTypes.ApplicationCsv, MediaTypes.ApplicationXlsx)]
        public async Task<IActionResult> Export(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "id",
            [FromQuery] string sortDirection = "asc",
            [FromQuery] string? search = null,            
            [FromQuery] string fileName = "")
        {
            try
            {
                var acceptHeader = Request.Headers["Accept"].ToString();

                if (string.IsNullOrWhiteSpace(acceptHeader))
                    {
                    _logger.LogWarning("Cabeçalho 'Accept' ausente na requisição de exportação.");

                    return BadRequest("Cabeçalho 'Accept' é obrigatório para determinar o formato de exportação.");
                }


                var fileResult = await _service.ExportPage(
                    page,
                    pageSize,
                    sortBy,
                    sortDirection,
                    search,
                    acceptHeader,
                    fileName);

                return fileResult;
            }
            catch (NotSupportedException ex)
            {
                _logger.LogError(ex, "Erro ao exportar: formato não suportado.");
                return StatusCode(StatusCodes.Status415UnsupportedMediaType, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado ao exportar.");
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um erro ao processar a exportação.");
            }
        }

        }
}
