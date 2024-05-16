using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xsoft.Dtos.Authentication;

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
        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(AuthRegisterDto request )
        {
            var response = await _authRepository.Register(
                new User { Email = request.email},  request.password
                );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(AuthLoginDto request)
        {
            var response = await _authRepository.Login(request.email , request.password);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
