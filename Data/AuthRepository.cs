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
        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        public async Task<ServiceResponse<string>> Login(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.ToLower().Equals(email.ToLower()));
            if (user == null)
            {
                response.Success = false;
                response.Message = $"User [{email}] not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Invalid email or password.";
            }
            else
            {
                response.Data = CreateToken(user.Email, user.Id.ToString(), "User");
            }
            return response;
        }

        public async Task<ServiceResponse<string>> LoginAdmin(string email, string password)
        {
            var response = new ServiceResponse<string>();
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Email.ToLower().Equals(email.ToLower()));
            if (admin == null)
            {
                response.Success = false;
                response.Message = $"Admin [{email}] not found.";
            }
            else if (!VerifyPasswordHash(password, admin.PasswordHash, admin.PasswordSalt))
            {
                response.Success = false;
                response.Message = "Invalid email or password.";
            }
            else
            {
                response.Data = CreateToken(admin.Email, admin.Id.ToString(), "Admin");
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            var errorMessages = new List<string>();

            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.']\w+)*$";
            if (!Regex.IsMatch(user.Email, emailPattern))
            {
                errorMessages.Add("Invalid email format");
            }

            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{6,}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                errorMessages.Add("Password must be at least 6 characters long and include at least one lowercase letter, one uppercase letter, one number, and one special character");
            }

            if (await UserExists(user.Email))
            {
                errorMessages.Add($"User with email {user.Email} already exists");
            }

            if (errorMessages.Count > 0)
            {
                response.Success = false;
                response.Message = string.Join(" ## ", errorMessages);
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            response.Data = user.Id;
            return response;
        }

        public async Task<ServiceResponse<int>> RegisterAdmin(Admin admin, string password)
        {
            var response = new ServiceResponse<int>();
            var errorMessages = new List<string>();

            string emailPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.']\w+)*$";
            if (!Regex.IsMatch(admin.Email, emailPattern))
            {
                errorMessages.Add("Invalid email format");
            }

            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{6,}$";
            if (!Regex.IsMatch(password, passwordPattern))
            {
                errorMessages.Add("Password must be at least 6 characters long and include at least one lowercase letter, one uppercase letter, one number, and one special character");
            }

            if (await AdminExists(admin.Email))
            {
                errorMessages.Add($"Admin with email {admin.Email} already exists");
            }

            if (errorMessages.Count > 0)
            {
                response.Success = false;
                response.Message = string.Join(" ## ", errorMessages);
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            admin.PasswordHash = passwordHash;
            admin.PasswordSalt = passwordSalt;

            _context.Admins.Add(admin);
            await _context.SaveChangesAsync();
            response.Data = admin.Id;
            return response;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> AdminExists(string email)
        {
            return await _context.Admins.AnyAsync(a => a.Email.ToLower() == email.ToLower());
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
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

        private string CreateToken(string email, string id, string role)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, id),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, role)
        };

            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if (appSettingsToken == null)
                throw new Exception("AppSettings Token is null");

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}