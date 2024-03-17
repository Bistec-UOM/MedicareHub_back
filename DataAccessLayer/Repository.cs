using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
            try {
                await _dbSet.AddAsync(item);
                await _dbContext.SaveChangesAsync();
            }catch (Exception){
                throw new Exception("Error");
            }
        }

        public async Task Delete(int id)
        {
            var obj = await Get(id);

            if (obj != null)
            {
                try{
                    _dbSet.Remove(obj);
                    await _dbContext.SaveChangesAsync();
                }catch(Exception){
                    throw new Exception("Error");
                }
            }
            else
            {
                throw new Exception("Doesn't Exixst");
            }
        }

        public async Task<List<T>> GetAll()
        {
            try{
                return await _dbSet.ToListAsync();
            }
            catch(Exception){
                throw new Exception("Error");
            }
        }

        public async Task<T> Get(int id)
        {
            try{
                return await _dbSet.FindAsync(id);
            }
            catch (Exception){
                throw new Exception("Error");
            }
        }

        public async Task Update(T entity)
        {
            try{
                _dbSet.Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception){
                throw new Exception("Error");
            }
        }

        public async Task<List<T>> GetByProp(string propName, object val)
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propName);
            var constant = Expression.Constant(val);
            var equals = Expression.Equal(property, constant);
            var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);

            try{
                return await _dbSet.Where(lambda).ToListAsync();
            }
            catch(Exception){
                throw new Exception("Error");
            }
        }

    }
}
