using RestWithAspNet10_Scaffold.Model;
using RestWithAspNet10_Scaffold.Repositories.Implementation;

namespace RestWithAspNet10_Scaffold.Services.Implementations
{
    public class PersonServicesImplementation : IPersonServices
    {
        private readonly IPersonRepository _repository;
       
        public PersonServicesImplementation(IPersonRepository repository)
        {
            _repository = repository;
        }

        public Person Create(Person person)
        {
            return _repository.Create(person);
        }

        public Person? FindById(long id)
        {
            return _repository.FindById(id);
        }

        public IEnumerable<Person> FindAll()
        {
            return _repository.FindAll();
        }   

        public Person? Update(Person person)
        {
            return _repository.Update(person);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
