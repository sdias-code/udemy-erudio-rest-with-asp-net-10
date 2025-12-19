using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Services.Implementations
{
    public interface IBookServices
    {
        Book Create(Book book);

        Book? FindById(long id);

        IEnumerable<Book> FindAll();

        Book Update(Book book);

        void Delete(long id);
    }
}
