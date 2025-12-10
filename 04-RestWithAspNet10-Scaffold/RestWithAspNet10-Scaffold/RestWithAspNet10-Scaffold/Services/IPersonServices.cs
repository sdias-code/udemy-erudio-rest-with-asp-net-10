using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface IPersonServices
    {
        Person Create(Person person);
        Person? FindById(long id);
        IEnumerable<Person> FindAll();
        Person? Update(Person person);
        void Delete(long id);

    }
}
