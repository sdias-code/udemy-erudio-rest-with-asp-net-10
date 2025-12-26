namespace RestWithAspNet10_Scaffold.Repositories
{
    public interface IGenericRepository<T> where T : class 
    {
        T Create(T entity);
        T? Update(T entity);
        void Delete(long id);
        T? FindById(long id);
        IEnumerable<T> FindAll();
        bool Exists(long id);
    }
}
