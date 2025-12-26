
using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Data;

namespace RestWithAspNet10_Scaffold.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dataset;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dataset = _context.Set<T>();
        }

        public T Create(T entity)
        {
            _dataset.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public T? Update(T entity)
        {
            var entry = _context.Entry(entity);
            var keyProp = entry.Property("Id");

            if (keyProp.CurrentValue == null)
                throw new InvalidOperationException("Entidade sem chave primária definida.");

            var key = (long)keyProp.CurrentValue;

            var current = _dataset.Find(key);

            if (current == null)
                return null;

            _context.Entry(current).CurrentValues.SetValues(entity);

            _context.SaveChanges();

            return current;
        }

        public void Delete(long id)
        {            
            var entity = _dataset.Find(id);

            if (entity != null)
            {
                _dataset.Remove(entity);
                _context.SaveChanges();
            }
        }

        public T? FindById(long id) 
            => _dataset.Find(id);


        public IEnumerable<T> FindAll()        
            => _dataset.AsNoTracking().ToList();
        
        public bool Exists(long id)
         => _dataset.Any(e => EF.Property<long>(e, "Id") == id);





    }
}
