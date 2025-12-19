using RestWithAspNet10_Scaffold.Data;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Repositories.Implementation
{
    public class BookRepository : IBookRepository
    {
        public readonly AppDbContext _context;
        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> FindAll()
        {
            return _context.Books.ToList();
        }

        public Book? FindById(long id)
        {
            return _context.Books.Find(id);
        }

        public Book Create(Book book)
        {
            _context.Add(book);
            _context.SaveChanges();
            return book;
        }

        public Book Update(Book book)
        {
            var existingBook = _context.Books.Find(book.Id);
            if (existingBook == null) return null;

            _context.Entry(existingBook).CurrentValues.SetValues(book);
            _context.SaveChanges();
            return book;
        }
        public void Delete(long id)
        {
            var existingBook = _context.Books.Find(id);
            if (existingBook == null) return;
            _context.Remove(existingBook);
            _context.SaveChanges();
        }

    }
}
