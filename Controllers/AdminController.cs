using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using xsoft.Dtos.Configuration;

namespace xsoft.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        [HttpGet("allConfigurations")]
        public async Task<ActionResult<ServiceResponse<string>>> getAllConfigurations(ConfigurationDto request)
        {
            
            return Ok();
        }
        [HttpGet("Configuration")]
        public async Task<ActionResult<ServiceResponse<string>>> getConfiguration(ConfigurationDto request)
        {
            return Ok();
        }

        [HttpPost("CreateConfiguration")]
        public async Task<ActionResult<ServiceResponse<string>>> AddConfiguration(ConfigurationDto request)
        {
            return Ok();
        }
        [HttpPatch("updateConfiguration")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateConfiguration(ConfigurationDto request)
        {
            return Ok();
        }

        [HttpDelete("DestroyConfiguration")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteConfiguration(ConfigurationDto request)
        {
            return Ok();
        }

        [HttpPost("ResetConfigurationOwner")]
        public async Task<ActionResult<ServiceResponse<string>>> RestConfigurationOwner(ConfigurationDto request)
        {
            return Ok();
        }

    }
}
