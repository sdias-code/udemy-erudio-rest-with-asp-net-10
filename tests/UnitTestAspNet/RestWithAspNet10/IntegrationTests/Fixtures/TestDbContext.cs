using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10.IntegrationTests.Fixtures
{
    public class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions options)
           : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}
