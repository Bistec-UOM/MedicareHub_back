using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _Repository;
        private readonly ApplicationDbContext dbContext;

        public UserService(IRepository<User> Repository, ApplicationDbContext dbContext)
        {
            _Repository = Repository;
            this.dbContext = dbContext; // Initialize dbContext
        }

        public async Task AddUser(User user)
        {
            await _Repository.Add(user);
        }

        public async Task DeleteUser(int id)
        {
            // Retrieve appointments associated with the user ID
            var appointmentsToDelete = await dbContext.appointments.Where(a => a.DoctorId == id).ToListAsync();
            if (appointmentsToDelete != null) // Fixed the comparison operator
            {
                // Delete each appointment
                foreach (var appointment in appointmentsToDelete)
                {
                    dbContext.appointments.Remove(appointment);
                }
            }
            await _Repository.Delete(id);
            // Save changes to persist the deletion
            await dbContext.SaveChangesAsync();
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
