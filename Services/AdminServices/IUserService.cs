using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AdminServices
{
    public interface IUserService
    {
        Task AddUser(User user);
        Task DeleteUser(int id);
        Task<User> GetUser(int id);
        Task<List<User>> GetAllUsers();
        Task<int> UpdateUser(User user);
    }
}
