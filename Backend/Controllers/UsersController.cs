
using Microsoft.AspNetCore.Mvc;
using Backend.Model;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        private IConfiguration _config;
        public UsersController(IConfiguration config, DataContext context)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public IActionResult GetUsers(string? username = "")
        {
            var users = _context.UserList!.AsQueryable();
            if (username != "")
            {
               users = users.Where(x => x.Username == username);
            }
            if (users == null) return NotFound();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public ActionResult<User> GetUser(Guid id)
        {
            var user = _context.UserList!.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound();
            return user;
        }

        [HttpPut("{id}")]
        public IActionResult PutUser(Guid id, User user)
        {
            if (!UserExists(id)) return NotFound();
            _context.Update(user);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPost("register")]
        public ActionResult<User> PostUser([FromBody] User register)
        {
            bool userExists = _context.UserList!.Any(a => a.Username == register.Username);
            if (userExists) return Conflict("Username is already exists");

            bool accountExists = _context.AccountList!.Any(a => a.Id == register.Id);
            if (!UserExists(register.Id) && !accountExists)
            {
                register.Password = HashPassword(register.Password);
                Account acc = new() { Id = register.Id };
                Facility facility = new() { OwnerId = register.Id};
                Room room = new() {FacilityId = facility.Id};

                _context.Add(register);
                _context.Add(acc);
                _context.Add(facility);
                _context.Add(room);
                _context.SaveChanges();

                return CreatedAtAction(nameof(GetUser), new { id = register.Id }, register);
            }
            else return Conflict();
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User login)
        {
            var dbUser = _context.UserList!.FirstOrDefault(user => user.Username == login.Username);

            if (dbUser == null) return NotFound();

            if (dbUser.Password != HashPassword(login.Password)) return Unauthorized();

            var token = GenerateJSONWebToken(dbUser);

            return Ok(new { token = token });

        }

        [HttpDelete("{id}")]
        public ActionResult<User> DeleteUser(Guid id)
        {
            var user = _context.UserList!.FirstOrDefault(x => x.Id == id);
            if (user == null) return NotFound();

            _context.UserList!.Remove(user);
            _context.SaveChanges();
            return Ok();
        }

        //private bool UserExists(string id) => _context.UserList!.Any(a => a.Id == Guid.Parse(id));
        private bool UserExists(Guid id) => _context.UserList!.Any(a => a.Id == id);
        private string HashPassword(string password)
        {
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: new byte[0],
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
        private string GenerateJSONWebToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              new List<Claim> { new Claim(ClaimTypes.Sid, user.Id.ToString()) },
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}