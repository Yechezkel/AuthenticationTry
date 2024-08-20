using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenticationTry.Data;
using AuthenticationTry.Models;

namespace AuthenticationTry.Controllers
{
    [Route("")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AuthenticationTryContext _context;

        public UsersController(AuthenticationTryContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        [HttpGet("home")]
        public async Task<ActionResult<string>> Home()
        {

            return Ok("Wellcome");
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> CreatUser([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            _context.User.Add(user);
            await _context.SaveChangesAsync();
            user.Password = "****";
            return CreatedAtAction(nameof(CreatUser),user.Id,user);
        }


        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody] User loginUser)
        {
            if (loginUser == null)
            {
                return BadRequest("invalid");
            }
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserName == loginUser.UserName && u.Password == loginUser.Password)!;
            if (user == null)
            {
                return Unauthorized("incorrect");
            }
            string token = Guid.NewGuid().ToString();
            user.Token = token;
            await _context.SaveChangesAsync();

            Response.Cookies.Append("auth", token, new CookieOptions { HttpOnly=true, SameSite=SameSiteMode.Strict, Secure = true,Expires = DateTimeOffset.UtcNow.AddHours(1) });
            return Ok("you are in");
        }

        [HttpGet("logout")]
        public async Task<ActionResult<User>> Logout()
        {
            Response.Cookies.Delete("auth");
            return Ok("you are out");
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
