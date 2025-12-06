using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Services.Implementations
{
    public class PersonServicesImpl : IPersonServices
    {
        public Person Create(Person person)
        {
            person.Id = new Random().Next(1, 1000);
            return person;

        }

        public void Delete(long id)
        {
            
        }

        public List<Person> FindAll()
        {
            var listPerson = ListMockPerson();

            return listPerson;

        }

        private List<Person> ListMockPerson()
        {
            var listPerson = new List<Person>();

            listPerson.Add(new Person(new Random().Next(1, 1000), "John", "Doe", "Mahatam 123 street", "Male"));
            listPerson.Add(new Person(new Random().Next(1, 1000), "Marcus", "Mara", "Xito 205 street", "Male"));
            listPerson.Add(new Person(new Random().Next(1, 1000), "Fabio", "Lara", "Cravia 409 street", "Male"));
            listPerson.Add(new Person(new Random().Next(1, 1000), "Fabricio", "Morrow", "Boroth 202 street", "Male"));

            return listPerson;
        }

        public Person FindById(long id)
        {
            var person = MockPerso();

            return person;

            }

        private Person MockPerso()
        {
            var person = new Person
            {

                Id = new Random().Next(1,1000),
                FirstName = "John",
                LastName = "Doe",
                Address = "Mahatam 123 street",
                Gender = "Male"
            };

            return person;
        }

        public Person Update(Person person)
        {
            return person;
        }
    }
}
