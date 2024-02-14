using DataAccessLayer;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _Repository;
        
        public UserService(IRepository<User> Repository)
        {
            _Repository = Repository;
        }
        public async Task AddUser(User user)
        {
            await _Repository.Add(user);
        }

        public void DeleteUser(int id)
        {
            _Repository.Delete(id);
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _Repository.GetAll();
        }

        public Task<User> GetUser(int id)
        {
            return _Repository.Get(id);
        }

        public async Task<int> UpdateUser(User user)
        {
            return await _Repository.Update(user);
        }
    }
}
