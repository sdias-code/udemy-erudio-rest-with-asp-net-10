using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.DTOs.V1.Person;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface IPersonService
    {
        Task<PersonResponseDTO?> FindByIdAsync(long id);

        Task<PagedResponse<PersonResponseDTO>> FindAllAsync(
            int page,
            int pageSize,
            string sortBy,
            string direction,
            string? search);

        Task<PersonResponseDTO?> Enable(long id);
        Task<PersonResponseDTO?> Disable(long id);
        Task<PersonResponseDTO> CreateAsync(PersonCreateDTO dto);
        Task<PersonResponseDTO?> UpdateAsync(long id, PersonUpdateDTO dto);
        Task<bool> DeleteAsync(long id);
    }
}
