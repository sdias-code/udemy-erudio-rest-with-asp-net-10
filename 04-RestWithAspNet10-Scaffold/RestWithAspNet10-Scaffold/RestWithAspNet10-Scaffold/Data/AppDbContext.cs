using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<Person> Persons { get; set; }
        
    }
}
