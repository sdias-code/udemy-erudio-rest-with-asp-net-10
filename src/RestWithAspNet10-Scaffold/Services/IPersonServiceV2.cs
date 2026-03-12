using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.DTOs.V2.Person;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface IPersonServiceV2
    {
        Task<PersonResponseDTO?> FindById(long id);

        Task<PagedResponse<PersonResponseDTO>> FindAll(
            int page,
            int pageSize,
            string sortBy,
            string direction,
            string? search);

        Task<PersonResponseDTO?> Enable(long id);
        Task<PersonResponseDTO?> Disable(long id);
        Task<PersonResponseDTO> Create(PersonCreateDTO dto);
        Task<PersonResponseDTO> Update(PersonUpdateDTO dto);
        Task Delete(long id);
    }
}
