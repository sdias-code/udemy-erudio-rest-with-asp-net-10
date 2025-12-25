
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

        public T Update(T entity)
        {
            _dataset.Update(entity);
            _context.SaveChanges();
            return entity;
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
            => _dataset.ToList();
        
        public bool Exists(long id)
         => _dataset.Any(e => EF.Property<long>(e, "Id") == id);





    }
}
