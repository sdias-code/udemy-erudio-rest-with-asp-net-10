using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Repositories
{
    public interface IBookRepository
    {
        Book? GetById(long id);
        List<Book> FindAll();
        Book Create(Book book);
        Book Update(Book book);
        void Delete(long id);
    }
}
