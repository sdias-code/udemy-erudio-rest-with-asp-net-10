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
        private readonly IPersonServices _personServices;
        private readonly ILogger<PersonController> _logger;


        public PersonController(IPersonServices personServices, ILogger<PersonController> logger)
        {
            _personServices = personServices;
            _logger = logger;
        }

        [HttpGet("{id:long}")]
        public IActionResult Get(long id)
        {
            var person = _personServices.FindById(id);          

            return Ok(person);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var persons = _personServices.FindAll();

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

            var createdPerson = _personServices.Create(person);

            return CreatedAtAction(nameof(Get), new { id = createdPerson.Id }, createdPerson);
        }

        [HttpPut("{id:long}")]
        public IActionResult Put(long id, [FromBody] Person person)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (person.Id != 0 && person.Id != id)
                return BadRequest("ID do corpo difere do ID da URL.");

            var existing = _personServices.FindById(id);
            if (existing == null)
                return NotFound();

            person.Id = id;

            var updatedPerson = _personServices.Update(person);

            return Ok(updatedPerson);
        }

        [HttpDelete("{id:long}")]
        public IActionResult Delete(long id)
        {
            var existing = _personServices.FindById(id);

            if (existing == null)
                return NotFound();

            _personServices.Delete(id);

            return NoContent();
        }
    }
}
