using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;
using RestWithAspNet10_Scaffold.Services;

namespace RestWithAspNet10_Scaffold.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [EnableCors("LocalPolicy")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonService _service;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonService service, ILogger<PersonController> logger)
        {
            _service = service;
            _logger = logger;
        }       

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(PersonResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PersonResponseDTO> Get(long id)
        {
            var person = _service.FindById(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<PersonResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<PersonResponseDTO>> Get()
        {
            var persons = _service.FindAll();

            _logger.LogInformation("Listando todas as pessoas cadastradas no banco.");           

            return Ok(persons);
        }

        [HttpPatch("{id:long}/enable")]
        [ProducesResponseType(typeof(PersonResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public ActionResult<PersonResponseDTO> Enable(long id)
        {
            var person = _service.Enable(id);

            if (person == null)
            {
                _logger.LogWarning("Tentativa de habilitar pessoa com ID {Id} falhou. Pessoa não encontrada.", id);
                return NotFound();
            }
            
            _logger.LogInformation("Pessoa com ID {Id} habilitada com sucesso.", id);

            return Ok(person);
        }

        [HttpPatch("{id:long}/disable")]
        [ProducesResponseType(typeof(PersonResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]       
        public ActionResult<PersonResponseDTO> Disable(long id)
        {
            var person = _service.Disable(id);
            if (person == null)
            {
                _logger.LogWarning("Tentativa de desabilitar pessoa com ID {Id} falhou. Pessoa não encontrada.", id);
                return NotFound();
            }
                
            _logger.LogInformation("Pessoa com ID {Id} desabilitada com sucesso.", id);

            return Ok(person);
        }

        [HttpPost]
        [ProducesResponseType(typeof(PersonResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Post([FromBody] PersonCreateDTO dto)
        {                     

            var createdPerson = _service.Create(dto);

            return CreatedAtAction(nameof(Get), new { id = createdPerson.Id }, createdPerson);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(typeof(PersonResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Put(long id, [FromBody] PersonUpdateDTO dto)
        {            

            if (id != dto.Id)
                return BadRequest("ID da rota diferente do body.");

            var existing = _service.FindById(id);
            if (existing == null)
                return NotFound();

            dto.Id = id;

            var updatedPerson = _service.Update(dto);

            return Ok(updatedPerson);
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
