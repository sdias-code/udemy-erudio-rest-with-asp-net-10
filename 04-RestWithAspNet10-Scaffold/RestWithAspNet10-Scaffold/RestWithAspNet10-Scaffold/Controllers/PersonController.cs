using Microsoft.AspNetCore.Mvc;
using RestWithAspNet10_Scaffold.Model;
using RestWithAspNet10_Scaffold.Services;

namespace RestWithAspNet10_Scaffold.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly IPersonServices _personServices;
        

        public PersonController(IPersonServices personServices)
        {
            _personServices = personServices;
        }

        [HttpGet("{id:long}")]
        public IActionResult Get(long id)
        {
            var person = _personServices.FindById(id);

            if (person == null)
                return NotFound();

            return Ok(person);
        }

        [HttpGet]
        public IActionResult Get()
        {
            var persons = _personServices.FindAll();

            if (persons == null || !persons.Any())
                return NotFound();

            return Ok(persons);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Person person)
        {
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

            // valida consistência do id
            if (id != person.Id)
                return BadRequest("ID do caminho não coincide com o ID do objeto.");

            var existing = _personServices.FindById(id);
            if (existing == null)
                return NotFound();

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
