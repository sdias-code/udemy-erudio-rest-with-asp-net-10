using RestWithAspNet10_Scaffold.DTOs.Book;

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
