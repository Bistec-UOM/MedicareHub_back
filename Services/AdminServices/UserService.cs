using BCrypt.Net;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace Services.AdminServices
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
            // Hash the password asynchronously before adding the user
            user.Password = await HashPassword(user.Password);

            // Add the user to the repository
            await _Repository.Add(user);

            // Based on user role, perform additional actions
            if (user.Role == "Doctor")
            {
                var doctor = new Doctor
                {
                    UserId = user.Id,
                };
                dbContext.doctors.Add(doctor);
            }
            else if (user.Role == "Admin")
            {
                var admin = new Admin
                {
                    UserId = user.Id,
                };
                dbContext.admins.Add(admin);
            }
            else if (user.Role == "Receptionist")
            {
                var receptionist = new Receptionist
                {
                    UserId = user.Id,
                };
                dbContext.receptionists.Add(receptionist);
            }
            else if (user.Role == "Lab Assistant")
            {
                var labAssistant = new LabAssistant
                {
                    UserId = user.Id,
                };
                dbContext.labAssistants.Add(labAssistant);
            }
            else if (user.Role == "Cashier")
            {
                var cashier = new Cashier
                {
                    UserId = user.Id,
                };
                dbContext.cashiers.Add(cashier);
            }

            // Save changes to the database
            await dbContext.SaveChangesAsync();
        }

        public async Task<string> HashPassword(string data)
        {
            // Hash the password using BCrypt.Net and return the hashed string
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(data);
            return hashedPassword;
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

        public async Task UpdateUser(User user)
        {
            // Check if the password is already hashed
            if (!user.Password.StartsWith("$2a$1"))
            {
                user.Password = await HashPassword(user.Password);
            }
            await _Repository.Update(user);
        }


    }
}
