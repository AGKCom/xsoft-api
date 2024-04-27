using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xsoft.Dtos.Client;

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
        public async Task<ActionResult<ServiceResponse<int>>> Register(ClientRegisterDto request )
        {
            var response = await _authRepository.Register(
                new Client { email = request.email},  request.password
                );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<int>>> Login(ClientLoginDto request)
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
