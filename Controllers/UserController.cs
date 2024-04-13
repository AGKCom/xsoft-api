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
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/v1/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpGet("FullUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsersWithConfigurations()
        {
            return await _context.Users
                .Include(u => u.UserConfigurations)
                    .ThenInclude(uc => uc.Configuration)
                .AsNoTracking()
                .ToListAsync();
        }

        // PUT: api/v1/User/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // GET: api/v1/User/FullUser/5
        [HttpGet("FullUser/{id}")]
        public async Task<ActionResult<User>> GetFullUserById(int id)
        {
            var user = await _context.Users
                .Where(u => u.id == id)
                .Include(u => u.UserConfigurations)
                    .ThenInclude(uc => uc.Configuration)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/v1/User
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.id }, user);
        }

        // DELETE: api/v1/User/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.id == id);
        }

        [HttpPost("test")]
        public async Task<IActionResult> TestConnection(Configuration configuration)
        {
            //try to establish sql server connection
            var connectionString = configuration.GetConnectionString();

            // Using Entity Framework Core's DbContext to test the connection
            var optionsBuilder = new DbContextOptionsBuilder<DbContext>();
            optionsBuilder.UseSqlServer(connectionString);
            
            // Try to open the database connection
            try
            {
                using (var context = new DbContext(optionsBuilder.Options))
                {
                    // If this succeeds, the connection is fine
                    await context.Database.ExecuteSqlRawAsync("SELECT 1");
                    return new ContentResult
                    {
                        ContentType = "application/json",
                        StatusCode = 200,
                        Content = "{\"success\":true,\"message\": \"Connection successful.\"}"
                    };
                }
            }
            catch (Exception ex)
            {
                // If an exception occurs, connection is not successful
                var message = "Connection failed: " + ex.Message.Replace('"', '\'').Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ");
                return new ContentResult
                {
                    ContentType = "application/json",
                    StatusCode = 200,
                    Content = "{\"success\":false,\"message\":\" "+ message + "\"}"
                };
            }
        }
    }
}
