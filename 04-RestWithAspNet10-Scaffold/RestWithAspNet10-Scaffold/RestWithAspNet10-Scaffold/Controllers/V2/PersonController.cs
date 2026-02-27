using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.DTOs.V2.Person;
using RestWithAspNet10_Scaffold.Services;


namespace RestWithAspNet10_Scaffold.Controllers.V2
{
    [Route("api/v2/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonController : ControllerBase
    {
        private readonly IPersonServiceV2 _service;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonServiceV2 service, ILogger<PersonController> logger)
        {
            _service = service;
            _logger = logger;
        }
       

        [HttpGet("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get(long id)
        {
            var person = _service.FindById(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<PersonResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PagedResponse<PersonResponseDTO>>> Get(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "id",
            [FromQuery] string direction = "asc",
            [FromQuery] string? search = null)
        {
            var result = await _service.FindAll(page, pageSize, sortBy, direction, search);

            if (!result.Items.Any())
                return NotFound();

            _logger.LogInformation("Listando todas as pessoas cadastradas no banco.");

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PersonResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]        
        public IActionResult Post([FromBody] PersonCreateDTO dto)
        {           

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPerson = _service.Create(dto);

            return CreatedAtAction(nameof(Get), new { id = createdPerson.Id }, createdPerson);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PersonResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Put(long id, [FromBody] PersonUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.Id != 0 && dto.Id != id)
                return BadRequest("ID do corpo difere do ID da URL.");

            var existing = _service.FindById(id);
            if (existing == null)
                return NotFound();

            dto.Id = id;

            var updatedPerson = _service.Update(dto);

            return Ok(updatedPerson);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Delete(long id)
        {
            var existing = _service.FindById(id);

            if (existing == null)
                return NotFound();

            _service.Delete(id);

            return NoContent();
        }
    }
}
