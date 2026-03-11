using Microsoft.EntityFrameworkCore;
using RestWithAspNet10_Scaffold.Data;
using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.Infrastructure.Query;
using RestWithAspNet10_Scaffold.Model;
using System.Linq.Expressions;

namespace RestWithAspNet10_Scaffold.Repositories.Implementation
{
    public class PersonRepository : IPersonRepository
    {
        protected readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Person?> GetByIdAsync(long id)
        {
            return await _context.Persons
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedResponse<Person>> FindAllAsync(
            int page,
            int pageSize,
            string sortBy,
            string direction,
            string? search)
        {
            IQueryable<Person> query = _context.Persons.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = $"%{search}%";
                query = query.Where(p =>
                    EF.Functions.Like(p.FirstName, term) ||
                    EF.Functions.Like(p.LastName, term));
            }

            query = query.ApplySorting(sortBy, direction, SortMap);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResponse<Person>
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                Items = items
            };
        }



        public async Task<Person> CreateAsync(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return person;
          
        }

        public async Task<List<Person>> CreateRangeAsync(List<Person> persons)
        {
            await _context.Persons.AddRangeAsync(persons);
            await _context.SaveChangesAsync();

            return persons;
        }

        public async Task<Person> UpdateAsync(Person person)
        {
            _context.Persons.Update(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity != null)
            {
                _context.Persons.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Person?> Disable(long id)
        {
            var entity = await _context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null) return null;

            entity.Enabled = false;
            await _context.SaveChangesAsync();

            return entity;
        }


        public async Task<Person?> Enable(long id)
        {
            var entity = await _context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);

            if (entity == null) return null;

            entity.Enabled = true;
            await _context.SaveChangesAsync();

            return entity;
        }

        private static readonly Dictionary<string, Expression<Func<Person, object?>>> SortMap
        = new(StringComparer.OrdinalIgnoreCase)
        {
            ["firstname"] = p => p.FirstName,
            ["lastname"] = p => p.LastName,
            ["gender"] = p => p.Gender,
            ["id"] = p => p.Id
        };

    }
    
}
