using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRepository<T> where T : class
    {
 
        Task Delete(int id);

        Task Add(T entity);
        Task<T> Get(int id);
        Task<List<T>> GetAll();
        Task Update(T entity);
    }
}
