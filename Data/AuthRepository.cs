using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public Task<ServiceResponse<string>> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            //check if user already exists
            var response = new ServiceResponse<int>();
            if (await UserExists(user.email))
            {
                response.Success = false;
                response.Message = user.email + " User Already exists";
                return response;
            }

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
    }
}