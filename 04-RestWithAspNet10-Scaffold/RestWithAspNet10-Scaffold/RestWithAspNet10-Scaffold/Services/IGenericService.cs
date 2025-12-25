namespace RestWithAspNet10_Scaffold.Services
{
    public interface IGenericService<T> where T : class
    {
        T Create(T entity);
        T Update(T entity);
        void Delete(long id);
        T? FindById(long id);
        IEnumerable<T> FindAll();
    }
}
