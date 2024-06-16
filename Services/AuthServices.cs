using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTO.Auth;
using SendGrid.Helpers.Mail;
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
            int? RoleId = null;

            if (tmp.Role == "Doctor") {
                RoleId = _cnt.doctors.SingleOrDefault(d => d.UserId == UserId)?.Id;
            }else if (tmp.Role == "Lab Assistant") {
                RoleId = _cnt.labAssistants.SingleOrDefault(l => l.UserId == UserId)?.Id;
            }else if (tmp.Role == "Cashier") {
                RoleId = _cnt.cashiers.SingleOrDefault(c => c.UserId == UserId)?.Id;
            }else if (tmp.Role == "Receptionist") {
                RoleId = _cnt.receptionists.SingleOrDefault(r => r.UserId == UserId)?.Id;
            }else {
                RoleId = 0;//Admin ID
            }

            List<Claim> claims = new List<Claim> {
                new Claim("Id",tmp.Id.ToString()),
                new Claim("Name", tmp.Name !),
                new Claim("Role", tmp.Role !),
                new Claim("RoleId",RoleId.ToString()),
                new Claim("IssuedAt", DateTime.UtcNow.ToString()),
                new Claim("Profile",tmp.ImageUrl)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!.PadRight(64, '\0')));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(330).AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        //OTP stages : ready --> vaild --> done / expired
        public async Task<string> SendOTP(int id)
        {
            var tmp = await _cnt.users.Where(e => e.Id == id).FirstOrDefaultAsync();
            if (tmp != null)
            {
                Otp obj = new Otp();

                Random RndNm = new Random();
                obj.code = RndNm.Next(100000, 999999);
                obj.status = "ready";
                obj.userId = id;
                obj.DateTime = DateTime.UtcNow;

                await _otp.Add(obj);

                string msg = obj.code + " is your verification code. Please use this to verify your identity before reset the password.";
                var sendMail = new EmailSender();
                await sendMail.SendMail("Reset password", "kwalskinick@gmail.com", tmp.Name, msg);
                return ("Check out your Email, A verification code is sent to " + tmp.Email);
            }
            else
            {
                return "";
            }

        }

        public async Task<String> CheckOTP(SentOTP data)
        {
            var otp=await _cnt.otps.OrderByDescending(e => e.Id).FirstOrDefaultAsync(o => o.userId == data.UserId);
            if(otp != null)
            {
                if (otp.status == "ready")
                {
                    if (otp.code == data.OTP)//otp verified
                    {
                        otp.status = "valid";
                        await _otp.Update(otp);
                        return "OK";
                    }
                    else
                    {
                        return "Incorrect OTP";
                    }
                }else
                {
                    return "OTP has expired. Try again";
                }
            }
            else
            {
                return "OTP Doen't exist";
            }

        }

        public async Task NewPassword(NewPassword data)
        {
            Otp? otp = await _cnt.otps.OrderByDescending(e => e.Id).FirstOrDefaultAsync(o => o.userId == data.UserId);
            if (otp.status == "valid")
            {
                User? user = await _cnt.users.Where(e => e.Id == data.UserId).FirstOrDefaultAsync();
                using (var transaction = await _cnt.Database.BeginTransactionAsync())
                {
                    otp.status = "done";
                    _cnt.otps.Update(otp);
                    user.Password = BCrypt.Net.BCrypt.HashPassword(data.Password);
                    _cnt.users.Update(user);
                    await _cnt.SaveChangesAsync();

                    await transaction.CommitAsync();
                }

            }

        }
    }
}
