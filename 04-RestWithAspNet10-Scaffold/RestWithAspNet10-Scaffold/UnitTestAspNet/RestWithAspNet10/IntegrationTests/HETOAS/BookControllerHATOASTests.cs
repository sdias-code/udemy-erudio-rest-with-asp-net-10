using RestWithAspNet10.IntegrationTests.Fixtures;
using RestWithAspNet10.IntegrationTests.Tools;
using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.DTOs.V1.Book;
using System.Text;
using System.Text.Json;

namespace RestWithAspNet10.IntegrationTests.HETOAS
{
    [Collection("IntegrationTests")]
    public class BookControllerHATOASTests : IClassFixture<SqlServerFixture>
    {
        private readonly HttpClient _client;
        private readonly TestDatabaseFixture _db;

        public BookControllerHATOASTests(SqlServerFixture sqlServerFixture, TestDatabaseFixture db)
        {
            var factory = new CustomWebApplicationFactory<Program>(
                sqlServerFixture.ConnectionString);

            _db = db;

            _db.InitializeAsync(sqlServerFixture.ConnectionString)
                .GetAwaiter().GetResult();

            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllBooks_ShouldReturnBooksWithHateoasLinks()
        {
            // Arrange
            await _db.ResetAsync();
            await _db.SeedAllAsync();

            // Act 
            var response = await _client.GetAsync("/api/v1/book");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var pagedBooks = JsonSerializer.Deserialize<PagedResponse<BookResponseDTO>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert 
            Assert.NotNull(pagedBooks);
            Assert.NotEmpty(pagedBooks.Items);

            var first = pagedBooks.Items[0];

            Assert.NotEmpty(first.Links);
            Assert.Contains(first.Links, l => l.Rel == "collection");
            Assert.Contains(first.Links, l => l.Rel == "self");
            Assert.Contains(first.Links, l => l.Rel == "create");
            Assert.Contains(first.Links, l => l.Rel == "update");
            Assert.Contains(first.Links, l => l.Rel == "delete");
        }

        [Fact]
        public async Task GetBookById_ShouldReturnBookWithHateoasLinks()
        {
            // Arrange
            await _db.ResetAsync();
            await _db.SeedAllAsync();

            // Act
            var response = await _client.GetAsync("/api/v1/book/1");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var book = JsonSerializer.Deserialize<BookResponseDTO>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.NotNull(book);
            Assert.NotEmpty(book.Links);

            Assert.Contains(book.Links, l => l.Rel == "self");
            Assert.Contains(book.Links, l => l.Rel == "update");
            Assert.Contains(book.Links, l => l.Rel == "delete");
        }

        [Fact]
        public async Task CreateBook_ShouldReturnBookWithHateoasLinks()
        {
            // Arrange
            await _db.ResetAsync();
            await _db.SeedAllAsync();

            var newBook = new
            {
                title = "Clean Architecture",
                author = "Robert C. Martin",
                price = 120,
                launchDate = DateTime.UtcNow
            };

            var content = new StringContent(
                JsonSerializer.Serialize(newBook),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/v1/book", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var created = JsonSerializer.Deserialize<BookResponseDTO>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.NotNull(created);
            Assert.True(created.Id > 0);
            Assert.NotEmpty(created.Links);

            Assert.Contains(created.Links, l => l.Rel == "self");
            Assert.Contains(created.Links, l => l.Rel == "update");
            Assert.Contains(created.Links, l => l.Rel == "delete");
        }

        [Fact]
        public async Task UpdateBook_ShouldReturnUpdatedBookWithHateoasLinks()
        {
            // Arrange
            await _db.ResetAsync();
            await _db.SeedAllAsync();

            var updatedBook = new
            {
                id = 1,
                title = "Updated Title",
                author = "Updated Author",
                price = 99,
                launchDate = DateTime.UtcNow
            };

            var content = new StringContent(
                JsonSerializer.Serialize(updatedBook),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PutAsync("/api/v1/book/1", content);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var book = JsonSerializer.Deserialize<BookResponseDTO>(
                json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // Assert
            Assert.NotNull(book);
            Assert.Equal("Updated Title", book.Title);
            Assert.NotEmpty(book.Links);

            Assert.Contains(book.Links, l => l.Rel == "self");
            Assert.Contains(book.Links, l => l.Rel == "delete");
        }

        [Fact]
        public async Task DeleteBook_ShouldRemoveBook()
        {
            // Arrange
            await _db.ResetAsync();
            await _db.SeedAllAsync();

            // Act
            var deleteResponse = await _client.DeleteAsync("/api/v1/book/1");

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NoContent, deleteResponse.StatusCode);

            var getResponse = await _client.GetAsync("/api/v1/book/1");
            Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
        }

    }
}
