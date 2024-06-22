using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using xsoft;
using xsoft.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using xsoft.models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace xsoft.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IAuthRepository _authRepository;

        public UserController(DataContext context, IAuthRepository authRepository)
        {
            _context = context;
            _authRepository = authRepository;
        }

        // GET: api/v1/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetCollaborators()
        {
            //get configuration that the user connected to 
            //return all colleborators 

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<User>>> AddCollaborator()
        {
            return Ok();
        }

        [HttpPatch]
        public async Task <ActionResult> resetCollaborator()
        {
            return Ok();
        }

        [HttpPatch]
        public async Task<ActionResult> activateOwner()
        {
            return Ok();
        }

        [HttpPatch]
        public async Task<ActionResult> activactivateCollaborator()
        {
            return Ok();
        }


    }
}
