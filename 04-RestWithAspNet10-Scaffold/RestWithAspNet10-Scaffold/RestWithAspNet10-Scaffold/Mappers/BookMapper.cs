using RestWithAspNet10_Scaffold.DTOs.Book;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Mappers
{
    public static class BookMapper
    {
        public static Book ToEntity(this BookCreateDTO dto)
        {
            return new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Price = dto.Price,
                LaunchDate = dto.LaunchDate
            };
        }

        public static BookResponseDTO ToDTO(this Book b)
        {
            return new BookResponseDTO
            {
                Id = b.Id,
                Title = b.Title!,
                Author = b.Author!,
                Price = b.Price,
                LaunchDate = b.LaunchDate
            };
        }
    }
}
