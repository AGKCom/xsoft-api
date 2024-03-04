using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using xsoft.Data;
using xsoft.models;

namespace xsoft
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
           var response = new ServiceResponse<string>();
           var user = await _context.users.FirstOrDefaultAsync(u => u.email.ToLower().Equals(email.ToLower()));
            if (user is null) 
            { 
                response.Success = false;
                response.Message = "User ["+ email+"] not fount.";
            }
            else if(!VerifyPasswordHash(password,user.passwordhash,user.passwordSalt)) 
            {
                response.Success= false;
                response.Message = "Invalid email or password.";
            }
            else
            {
                response.Data = user.id.ToString();
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            var errorMessages = new List<string>();

            // Email regex pattern
            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            if (!Regex.IsMatch(user.email, emailPattern))
            {
                errorMessages.Add("Invalid email format");
            }

            // Password regex pattern
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{6,}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                errorMessages.Add("Password must be at least 6 characters long and include at least one lowercase letter, one uppercase letter, one number, and one special character");
            }

            if (await UserExists(user.email))
            {
                errorMessages.Add($"{user.email} User already exists");
            }

            // If there are any error messages, concatenate them and return
            if (errorMessages.Count > 0)
            {
                response.Success = false;
                // Join the error messages using a colon and a space as separators
                response.Message = string.Join(" ## ", errorMessages);
                return response;
            }

            // If validation passed, proceed with user registration
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.passwordhash = passwordHash;
            user.passwordSalt = passwordSalt;

            _context.users.Add(user);
            await _context.SaveChangesAsync();
            response.Data = user.id;
            return response;
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.users.AnyAsync(u=>u.email.ToLower()==email.ToLower()))
            {
                return true;
            }
            return false;
        }

        private void CreatePasswordHash(string password,out byte[] passwordHash,out byte[] passwordSalt ) 
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}