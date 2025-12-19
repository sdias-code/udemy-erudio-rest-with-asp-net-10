using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Repositories.Implementation
{
    public interface IBookRepository
    {
        Book Create(Book book);

        Book? FindById(long id);

        IEnumerable<Book> FindAll();

        Book Update(Book book);

        void Delete(long id);
    }
}
