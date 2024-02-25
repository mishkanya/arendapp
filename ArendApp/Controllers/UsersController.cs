using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArendApp.Models;
using ArendApp.Api.Services;

namespace ArendApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly CodeSender _codeSender;

        public UsersController(ApplicationDbContext context, CodeSender codeSender)
        {
            _context = context;
            _codeSender = codeSender;
        }

        // GET: api/Users
        [HeaderValidator()]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsersData()
        {
            return await _context.UsersData.ToListAsync();
        }

        [HttpPost("/login")]
        public async Task<ActionResult<User>> Login(User user)
        {
            var dbUser = await _context.UsersData.FirstOrDefaultAsync(t => t.Email == user.Email && t.Password == user.Password);
            if (dbUser == null)
            {
                return BadRequest();
            }
            return Ok(dbUser);
        }


        // GET: api/Users/5
        [HttpGet("{id}")]
        [HeaderValidator()]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.UsersData.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [HeaderValidator()]
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
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.UsersData.Add(user);
            await _context.SaveChangesAsync();

            await _codeSender.SendCode(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        [HeaderValidator()]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.UsersData.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.UsersData.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/{code}")]
        [HeaderValidator()]
        public async Task<ActionResult<User>> ConfirmEmail(int id, int code)
        {
            var user = await _context.UsersData.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.UsersData.Any(e => e.Id == id);
        }
    }
}
