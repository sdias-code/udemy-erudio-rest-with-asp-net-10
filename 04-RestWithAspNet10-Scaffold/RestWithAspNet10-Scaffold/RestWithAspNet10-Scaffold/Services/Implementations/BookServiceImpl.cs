using RestWithAspNet10_Scaffold.DTOs.Book;
using RestWithAspNet10_Scaffold.Mappers;
using RestWithAspNet10_Scaffold.Repositories;

namespace RestWithAspNet10_Scaffold.Services.Implementations
{
    public class BookServiceImpl : IBookService
    {
        private readonly IBookRepository _repo;

        public BookServiceImpl(IBookRepository repo)
        {
            _repo = repo;
        }

        public BookResponseDTO? FindById(long id)
        {
            var entity = _repo.GetById(id);
            return entity?.ToDTO();
        }

        public List<BookResponseDTO> FindAll()
        {
            return _repo.FindAll()
                        .Select(b => b.ToDTO())
                        .ToList();
        }

        public BookResponseDTO Create(BookCreateDTO dto)
        {
            var entity = dto.ToEntity();
            return _repo.Create(entity).ToDTO();
        }

        public BookResponseDTO Update(BookUpdateDTO dto)
        {
            var entity = _repo.GetById(dto.Id);
            if (entity == null)
                throw new Exception("Livro não encontrado");

            entity.Title = dto.Title;
            entity.Author = dto.Author;
            if (dto.Price.HasValue) entity.Price = dto.Price.Value;

            return _repo.Update(entity).ToDTO();
        }

        public void Delete(long id) => _repo.Delete(id);
    }
}
