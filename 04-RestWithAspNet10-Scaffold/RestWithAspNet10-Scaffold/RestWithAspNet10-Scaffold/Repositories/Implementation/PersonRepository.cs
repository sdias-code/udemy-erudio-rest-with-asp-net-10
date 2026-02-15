using RestWithAspNet10_Scaffold.Data;
using RestWithAspNet10_Scaffold.DTOs.Common;
using RestWithAspNet10_Scaffold.Model;
using Microsoft.EntityFrameworkCore;

namespace RestWithAspNet10_Scaffold.Repositories.Implementation
{
    public class PersonRepository : IPersonRepository
    {
        protected readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Person?> GetById(long id)
        {
            return await _context.Persons
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PagedResponse<Person>> FindAll(
            int page,
            int pageSize,
            string sortBy,
            string direction,
            string? search)
        {
            var query = _context.Persons.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.ToLower();

                query = query.Where(p =>
                    p.FirstName.ToLower().Contains(term) ||
                    p.LastName.ToLower().Contains(term));
            }

            var isAsc = direction.ToLower() == "asc";

            query = sortBy.ToLower() switch
            {
                "firstname" => isAsc
                    ? query.OrderBy(p => p.FirstName)
                    : query.OrderByDescending(p => p.FirstName),

                "lastname" => isAsc
                    ? query.OrderBy(p => p.LastName)
                    : query.OrderByDescending(p => p.LastName),

                "gender" => isAsc
                    ? query.OrderBy(p => p.Gender)
                    : query.OrderByDescending(p => p.Gender),

                _ => isAsc
                    ? query.OrderBy(p => p.Id)
                    : query.OrderByDescending(p => p.Id)
            };

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

        public async Task<Person> Create(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<Person> Update(Person person)
        {
            _context.Persons.Update(person);
            await _context.SaveChangesAsync();
            return person;
        }


        public async Task Delete(long id)
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


    }
}
