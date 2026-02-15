using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Repositories
{
    public interface IPersonRepository
    {
        Task<Person?> GetById(long id);

        Task<PagedResponse<Person>> FindAll(
            int page,
            int pageSize,
            string sortBy,
            string direction,
            string? search);

        Task<Person> Create(Person person);

        Task<Person> Update(Person person);

        Task Delete(long id);

        Task<Person?> Disable(long id);

        Task<Person?> Enable(long id);
    }
}
