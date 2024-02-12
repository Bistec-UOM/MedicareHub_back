using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class DBop<T> : IDBop<T> where T : class
    {
        private readonly ApplicationDbContext _dbCon;
        private readonly DbSet<T> _dbSet;

        public DBop(ApplicationDbContext dbcontext)
        {
            _dbCon = dbcontext;
            _dbSet = _dbCon.Set<T>();
        }

        public async Task<T> Get(int id)
        {

                return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task Add(T item)
        {
            await _dbSet.AddAsync(item);
            await _dbCon.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            _dbSet.Remove(await Get(id));
            await _dbCon.SaveChangesAsync();

        }

        public async Task Update(T item)
        {
            _dbSet.Update(item);
            await _dbCon.SaveChangesAsync();
        }
    }
}
