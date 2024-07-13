using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using xsoft.Dtos.Authentication;
using xsoft.Data;

namespace xsoft.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(DataContext context, IConfiguration configuration, IAuthRepository authRepository)
        {
            _context = context;
            _authRepository = authRepository;
            _configuration = configuration;
        }

        [HttpPost("registerAdmin")]
        public async Task<ActionResult<ServiceResponse<int>>> RegisterAdmin(AdminRegisterDto request)
        {
            var response = await _authRepository.RegisterAdmin(
                new Admin { Email = request.Email }, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("authenticateAdmin")]
        public async Task<ActionResult<ServiceResponse<string>>> AuthenticateAdmin(AdminLoginDto request)
        {
            var response = await _authRepository.LoginAdmin(request.Email, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("registerUser")]
        public async Task<ActionResult<ServiceResponse<int>>> RegisterUser(UserRegisterDto request)
        {
            var response = await _authRepository.Register(
                new User { Email = request.Email ,Type = request.Type}, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("authenticateUser")]
        public async Task<ActionResult<ServiceResponse<string>>> AuthenticateUser(UserLoginDto request)
        {
            var response = await _authRepository.Login(request.Email, request.Password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [Authorize]
        [HttpPost("signOut")]
        public async Task<ActionResult> SignOut()
        {
            var result = await SignOutUserOrAdmin();
            if (!result)
            {
                return BadRequest(new { message = "Failed to sign out" });
            }
            return Ok(new { message = "Signed out successfully" });
        }

        private async Task<bool> SignOutUserOrAdmin()
        {
            if (!HttpContext.Request.Headers.ContainsKey("Authorization"))
            {
                return false;
            }

            var token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ").Last();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var email = principal.FindFirst(ClaimTypes.Email)?.Value;
                var role = principal.FindFirst(ClaimTypes.Role)?.Value;

                if (email == null || role == null)
                {
                    return false;
                }

                var identityData = new { email = email, role = role };
                var identityJson = JsonSerializer.Serialize(identityData);

                var authentication = await _context.Authentications
                    .SingleOrDefaultAsync(a => a.IdentityJson == identityJson);

                if (authentication != null)
                {
                    _context.Authentications.Remove(authentication);
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
