using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using xsoft.Data;
using xsoft.models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Security.AccessControl;
using System.IdentityModel.Tokens.Jwt;

namespace xsoft
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        public AuthRepository(DataContext context,IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }
        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
           var response = new ServiceResponse<string>();
           var user = await _context.Users.FirstOrDefaultAsync(u => u.email.ToLower().Equals(email.ToLower()));
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
            else if (AccountExpired(user))
            {
                response.Success = false;
                response.Message = "This Account is expired.";
            }
            else
            {
                response.Data = CreateToken(user);
            }
            return response;
        }

        private bool AccountExpired(User user)
        {
            return user.expirationDate < DateTime.UtcNow;
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

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            response.Data = user.id;
            return response;
        }

        public async Task<bool> UserExists(string email)
        {
            if (await _context.Users.AnyAsync(u=>u.email.ToLower()==email.ToLower()))
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
        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.id.ToString()),
                new Claim(ClaimTypes.Email,user.email)
            };
            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if (appSettingsToken is null)
                throw new Exception("AppSettings Token is null");
            
            SymmetricSecurityKey key = new SymmetricSecurityKey (System.Text.Encoding.UTF8.GetBytes (appSettingsToken));
            SigningCredentials creds = new SigningCredentials (key,SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken (tokenDescriptor);
            return tokenHandler.WriteToken (token);

        }
    }
}