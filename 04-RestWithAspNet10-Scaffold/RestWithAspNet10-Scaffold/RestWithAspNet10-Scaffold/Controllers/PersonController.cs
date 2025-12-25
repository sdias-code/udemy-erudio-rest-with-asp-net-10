using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.Model;
using RestWithAspNet10_Scaffold.Services;
using Serilog;

namespace RestWithAspNet10_Scaffold.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IGenericService<Person> _service;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IGenericService<Person> service, ILogger<PersonController> logger)
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

        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
            if (person.Id != 0)
                return BadRequest("ID não deve ser informado na criação.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdPerson = _service.Create(person);

            return CreatedAtAction(nameof(Get), new { id = createdPerson.Id }, createdPerson);
        }

        [HttpPut("{id:long}")]
        public IActionResult Put(long id, [FromBody] Person person)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (person.Id != 0 && person.Id != id)
                return BadRequest("ID do corpo difere do ID da URL.");

            var existing = _service.FindById(id);
            if (existing == null)
                return NotFound();

            person.Id = id;

            var updatedPerson = _service.Update(person);

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
