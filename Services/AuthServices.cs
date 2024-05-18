using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTO;
using Services.AppointmentService;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthServices
    {
        private readonly IRepository<User> _user;
        private readonly IConfiguration _configuration;
        private readonly IRepository<Otp> _otp;
        private readonly ApplicationDbContext _cnt;

        public AuthServices(IRepository<User> user, IConfiguration configuration, IRepository<Otp> otp, ApplicationDbContext cnt)
        {
            _user = user;
            _configuration = configuration;
            _otp = otp;
            _cnt = cnt;
        }

        public String RegisterUser(String data)
        {
            string paswrdHash = BCrypt.Net.BCrypt.HashPassword(data);
            data= paswrdHash;
            //await _user.Add(data);
            return data;
        }


        //Checking the validity of user-----------------------------------------------------
        public async Task<string> CheckUser(UserLog data)
        {
            var tmp =await _user.Get(data.UserId);
            if(tmp != null) 
            {
                if (BCrypt.Net.BCrypt.Verify(data.Password, tmp.Password))
                {
                    return "Valid";
                }
                else
                {
                    return "Invalid Password";
                }
            }
            else
            {
                return "Invalid User Id";
            }

        }


        //JWT token issuing------------------------------------------------------------
        public async Task<string> CreateToken(int UserId)
        {
            var tmp = await _user.Get(UserId);

            List<Claim> claims = new List<Claim> {
                new Claim("Id",tmp.Id.ToString()),
                new Claim("Name", tmp.Name !),
                new Claim("Role", tmp.Role !),
                new Claim("IssuedAt", DateTime.UtcNow.ToString()),
                new Claim("Profile",tmp.ImageUrl)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!.PadRight(64, '\0')));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        public async Task<string> VerifyCode(int id)
        {
            var tmp = await _user.Get(id);
            Otp obj = new Otp();

            Random RndNm = new Random();
            obj.code = RndNm.Next(100000, 999999);
            obj.status = "ready";
            obj.userId = id;

            await _otp.Add(obj);

            string msg = obj.code+ " is your verification code. Please use this to verify your identity before reset the password.";
            var sendMail = new EmailSender();
            await sendMail.SendMail("Reset password","kwalskinick@gmail.com",tmp.Name,msg);
            return ("Check out your Email, A verification code is sent to "+tmp.Email);
        }

        public async Task<Boolean> ConfirmCode(int Uid,int code)
        {
            var otp=await _cnt.otps.FirstOrDefaultAsync(o => o.userId == Uid && o.status == "ready");
            if (code == otp.code)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
