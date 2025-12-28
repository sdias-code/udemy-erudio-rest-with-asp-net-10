using RestWithAspNet10_Scaffold.Data;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Repositories.Implementation
{
    public class BookRepository : IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public Book? GetById(long id)
        {
            return _context.Books.FirstOrDefault(b => b.Id == id);
        }

        public List<Book> FindAll()
        {
            return _context.Books.ToList();
        }

        public Book Create(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return book;
        }

        public Book Update(Book book)
        {
            _context.Books.Update(book);
            _context.SaveChanges();
            return book;
        }

        public void Delete(long id)
        {
            var entity = _context.Books.FirstOrDefault(b => b.Id == id);
            if (entity != null)
            {
                _context.Books.Remove(entity);
                _context.SaveChanges();
            }
        }
    }
}
