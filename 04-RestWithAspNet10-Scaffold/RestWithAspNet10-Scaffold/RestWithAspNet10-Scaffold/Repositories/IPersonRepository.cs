using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Repositories
{
    public interface IPersonRepository
    {
        Task<Person?> GetByIdAsync(long id);

        Task<PagedResponse<Person>> FindAllAsync(
            int page,
            int pageSize,
            string sortBy,
            string direction,
            string? search);

        Task<Person> CreateAsync(Person person);

        Task<List<Person>> CreateRangeAsync(List<Person> persons);

        Task<Person> UpdateAsync(Person person);

        Task DeleteAsync(long id);

        Task<Person?> Disable(long id);

        Task<Person?> Enable(long id);
    }
}
