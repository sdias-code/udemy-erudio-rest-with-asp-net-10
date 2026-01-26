using RestWithAspNet10_Scaffold.DTOs.V1.Book;

namespace RestWithAspNet10_Scaffold.Services
{
    public interface IBookService
    {
        BookResponseDTO? FindById(long id);
        List<BookResponseDTO> FindAll();
        BookResponseDTO Create(BookCreateDTO dto);
        BookResponseDTO Update(BookUpdateDTO dto);
        void Delete(long id);
    }
}
