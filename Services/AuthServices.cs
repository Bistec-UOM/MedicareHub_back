using DataAccessLayer;
using Models;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthServices
    {
        private readonly IRepository<User> _user;

        public AuthServices(IRepository<User> user)
        {
            _user = user;
        }

        public async Task<User> RegisterUser(User data)
        {
            string paswrdHash = BCrypt.Net.BCrypt.HashPassword(data.Password);
            data.Password = paswrdHash;
            await _user.Add(data);
            return data;
        }

        public async Task<string> CheckUser(UserLog data)
        {
            var tmp =await _user.Get(data.UserId);
            if(BCrypt.Net.BCrypt.Verify(data.Password, tmp.Password))
            {
                return "OK";
            }
            else
            {
                return "Bad";
            }
        }
    }
}
