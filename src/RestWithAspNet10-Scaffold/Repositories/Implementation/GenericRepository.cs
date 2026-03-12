
using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Model.Base;

namespace RestWithAspNet10_Scaffold.Repositories.Implementation
{
    public class GenericRepository<T, TContext>
        : IGenericRepository<T>
        where T : BaseEntity
        where TContext : DbContext
    {
        protected readonly TContext _context;
        protected readonly DbSet<T> _dataset;

        public GenericRepository(TContext context)
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
            var current = _dataset.Find(entity.Id);

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


        public IQueryable<T> FindAll()
            => _dataset.AsNoTracking();

        public bool Exists(long id)
         => _dataset.Any(e => e.Id == id);

    }
}