using RestWithAspNet10_Scaffold.Data;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Repositories.Implementation
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public Person? GetById(long id)
        {
            return _context.Persons.FirstOrDefault(p => p.Id == id);
        }

        public List<Person> FindAll()
        {
            return _context.Persons.ToList();
        }

        public Person Create(Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();
            return person;
        }

        public Person Update(Person person)
        {
            _context.Persons.Update(person);
            _context.SaveChanges();
            return person;
        }

        public void Delete(long id)
        {
            var entity = _context.Persons.FirstOrDefault(p => p.Id == id);
            if (entity != null)
            {
                _context.Persons.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
