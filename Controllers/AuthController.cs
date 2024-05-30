using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xsoft.Dtos.Authentication;
using xsoft.models;

namespace xsoft.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
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
                new User { Email = request.Email }, request.Password
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

        [Authorize(Roles = "Admin")]
        [HttpPost("signOutAdmin")]
        public ActionResult SignOutAdmin()
        {
            // Logic to invalidate the token can be implemented here
            // This might involve maintaining a blacklist of tokens or simply removing them client-side
            return Ok(new { message = "Admin signed out" });
        }

        [Authorize(Roles = "User")]
        [HttpPost("signOutUser")]
        public ActionResult SignOutUser()
        {
            // Logic to invalidate the token can be implemented here
            // This might involve maintaining a blacklist of tokens or simply removing them client-side
            return Ok(new { message = "User signed out" });
        }
    }
}
