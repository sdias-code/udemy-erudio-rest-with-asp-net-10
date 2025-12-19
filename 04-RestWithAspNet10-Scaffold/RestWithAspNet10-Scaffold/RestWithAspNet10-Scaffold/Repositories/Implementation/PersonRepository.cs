using RestWithAspNet10_Scaffold.Data;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Repositories.Implementation
{
    public class PersonRepository : IPersonRepository
    {
        private AppDbContext _context;
        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public Person Create(Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();
            return person;
        }

        public void Delete(long id)
        {
            var person = _context.Persons.Find(id);
            if (person != null)
            {
                _context.Persons.Remove(person);
                _context.SaveChanges();
            }
        }

        public Person? FindById(long id)
        {
            var person = _context.Persons.Find(id);

            return person;
        }

        public IEnumerable<Person> FindAll()
        {
            return _context.Persons.ToList();
        }        

        public Person? Update(Person person)
        {
            var existingPerson = _context.Persons.Find(person.Id);

            if (existingPerson == null)
                return null;

            // Atualiza campos manualmente (mais seguro)
            existingPerson.FirstName = person.FirstName;
            existingPerson.LastName = person.LastName;
            existingPerson.Address = person.Address;
            existingPerson.Gender = person.Gender;

            //_context.Entry(existingPerson).CurrentValues.SetValues(person);

            _context.SaveChanges();
            return existingPerson;
        }

    }
}
