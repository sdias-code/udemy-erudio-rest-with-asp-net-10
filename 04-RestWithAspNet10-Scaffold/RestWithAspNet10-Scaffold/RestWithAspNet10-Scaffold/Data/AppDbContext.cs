using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Model;

namespace RestWithAspNet10_Scaffold.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<LogEvent> Logs { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Tabela de logs
            modelBuilder.Entity<LogEvent>(entity =>
            {
                entity.ToTable("Logs");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Message).HasColumnType("nvarchar(max)");
                entity.Property(e => e.MessageTemplate).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Exception).HasColumnType("nvarchar(max)");
                entity.Property(e => e.Properties).HasColumnType("nvarchar(max)");
            });
        }
    }
}
