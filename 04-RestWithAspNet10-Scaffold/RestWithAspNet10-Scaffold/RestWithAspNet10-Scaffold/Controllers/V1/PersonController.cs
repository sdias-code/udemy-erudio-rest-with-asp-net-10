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
        public IActionResult Get(long id)
        {
            var person = _service.FindById(id);          

            return Ok(person);
        }

        [HttpGet]        
        public IActionResult Get()
        {
            var persons = _service.FindAll();

            _logger.LogInformation("Listando todas as pessoas cadastradas no banco.");


            if (persons == null || !persons.Any())
                return NotFound();

            return Ok(persons);
        }

        [HttpPatch("{id:long}/enable")]
        public IActionResult Enable(long id)
        {
            var person = _service.Enable(id);
            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpPatch("{id:long}/disable")]
        public IActionResult Disable(long id)
        {
            var person = _service.Disable(id);
            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpPost]
        public IActionResult Post([FromBody] PersonCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);           

            var createdPerson = _service.Create(dto);

            return CreatedAtAction(nameof(Get), new { id = createdPerson.Id }, createdPerson);
        }

        [HttpPut("{id:long}")]
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
