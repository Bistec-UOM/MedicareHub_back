using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private DbSet<T> _dbSet;

        public Repository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }



        public async Task Add(T item)
        {
            await _dbSet.AddAsync(item);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var requiredObjectToDelete = await Get(id);

            if (requiredObjectToDelete != null)
            {
                _dbSet.Remove(requiredObjectToDelete);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                // Handle the case where the entity with the given id doesn't exist.
                // For now, I'm just logging to the console as an example.
                Console.WriteLine($"Entity with id {id} not found. Delete operation aborted.");
            }
        }




        public async Task<List<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> Get(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
        }
    }
}
