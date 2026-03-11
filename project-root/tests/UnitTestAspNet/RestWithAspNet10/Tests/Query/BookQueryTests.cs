using Microsoft.EntityFrameworkCore;
using RestWithAspNet10.IntegrationTests.Fixtures;
using RestWithAspNet10_Scaffold.Infrastructure.Query;
using RestWithAspNet10_Scaffold.Model;
using System.Linq.Expressions;

namespace RestWithAspNet10.Tests.Query
{
    public class BookQueryTests
    {
        private readonly Dictionary<string, Expression<Func<Book, object?>>> _sortMap =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["title"] = b => b.Title,
                ["author"] = b => b.Author,
                ["price"] = b => b.Price,
                ["launchdate"] = b => b.LaunchDate,
                ["id"] = b => b.Id
            };

        private DbContextOptions<TestDbContext> CreateOptions()
        {
            return new DbContextOptionsBuilder<TestDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        private async Task SeedAsync(TestDbContext context)
        {
            context.Books.AddRange(
                new Book
                {
                    Id = 1,
                    Title = "C# Advanced",
                    Author = "Silvio",
                    Price = 100,
                    LaunchDate = new DateTime(2024, 1, 1)
                },
                new Book
                {
                    Id = 2,
                    Title = "ASP.NET Core",
                    Author = "Maria",
                    Price = 200,
                    LaunchDate = new DateTime(2023, 1, 1)
                },
                new Book
                {
                    Id = 3,
                    Title = "Entity Framework",
                    Author = "Silvio",
                    Price = 150,
                    LaunchDate = new DateTime(2022, 1, 1)
                }
            );

            await context.SaveChangesAsync();
        }

        [Fact]
        public async Task ApplySorting_Should_Order_By_Price_Ascending()
        {
            var options = CreateOptions();

            using var context = new TestDbContext(options);
            await SeedAsync(context);

            var result = context.Books
                .ApplySorting("price", "asc", _sortMap)
                .ToList();

            Assert.Equal(100, result[0].Price);
            Assert.Equal(150, result[1].Price);
            Assert.Equal(200, result[2].Price);
        }

        [Fact]
        public async Task ApplySorting_Should_Order_By_Price_Descending()
        {
            var options = CreateOptions();

            using var context = new TestDbContext(options);
            await SeedAsync(context);

            var result = context.Books
                .ApplySorting("price", "desc", _sortMap)
                .ToList();

            Assert.Equal(200, result[0].Price);
            Assert.Equal(150, result[1].Price);
            Assert.Equal(100, result[2].Price);
        }

        [Fact]
        public async Task ApplyFilters_Should_Filter_By_Author_And_MinPrice()
        {
            var options = CreateOptions();

            using var context = new TestDbContext(options);
            await SeedAsync(context);

            var result = context.Books
                .ApplyFilters(
                    search: "Silvio",
                    launchFrom: null,
                    launchTo: null,
                    minPrice: 120,
                    maxPrice: null,
                    sortBy: "id",
                    direction: "asc",
                    sortMap: _sortMap)
                .ToList();

            Assert.Single(result);
            Assert.Equal("Entity Framework", result[0].Title);
        }

        [Fact]
        public async Task ApplyFilters_Should_Filter_By_LaunchDate_Range()
        {
            var options = CreateOptions();

            using var context = new TestDbContext(options);
            await SeedAsync(context);

            var result = context.Books
                .ApplyFilters(
                    search: null,
                    launchFrom: new DateTime(2023, 1, 1),
                    launchTo: new DateTime(2024, 12, 31),
                    minPrice: null,
                    maxPrice: null,
                    sortBy: "id",
                    direction: "asc",
                    sortMap: _sortMap)
                .ToList();

            Assert.Equal(2, result.Count);
        }
    }
    
}
