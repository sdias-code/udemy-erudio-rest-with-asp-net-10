using RestWithAspNet10_Scaffold.Repositories;

namespace RestWithAspNet10_Scaffold.Services.Implementations
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public T Create(T entity) => _repository.Create(entity);
        public T Update(T entity) => _repository.Update(entity);
        public void Delete(long id) => _repository.Delete(id);
        public T? FindById(long id) => _repository.FindById(id);
        public IEnumerable<T> FindAll() => _repository.FindAll();

    }
}
