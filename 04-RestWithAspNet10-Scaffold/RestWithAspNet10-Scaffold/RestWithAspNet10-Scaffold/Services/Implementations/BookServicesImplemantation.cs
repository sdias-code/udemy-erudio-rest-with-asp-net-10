using RestWithAspNet10_Scaffold.Model;
using RestWithAspNet10_Scaffold.Repositories.Implementation;

namespace RestWithAspNet10_Scaffold.Services.Implementations
{
    public class BookServicesImplemantation : IBookServices
    {
        private IBookRepository _repository;

        public BookServicesImplemantation(IBookRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Book> FindAll()
        {
            return _repository.FindAll();
        }

        public Book? FindById(long id)
        {
            return _repository.FindById(id);
        }

        public Book Create(Book book)
        {
            return _repository.Create(book);
        }

        public Book Update(Book book)
        {
            return _repository.Update(book);
        }
        public void Delete(long id)
        {
            _repository.Delete(id);
        }
    }
}
