using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArendApp.Api.Services;
using ArendApp.Models;
using ArendApp.Api.Extensions;

namespace ArendApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [HeaderValidator()]
    public class UsersBasketController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersBasketController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UsersBasket
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBasket>>> GetHistories()
        {
            var user = await this.GetUserAsync();
            return await _context.UsersBasket.Where(t => t.UsedId == user.Id).ToListAsync();
        }

        [HeaderValidator(true)]
        [HttpGet("All")]
        public async Task<IEnumerable<UserBasket>> GetAllHistories()
        {
            return await _context.UsersBasket.ToListAsync();
        }

        // GET: api/UsersBasket/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserBasket>> GetHistory(int id)
        {
            var usersBasket = await _context.UsersBasket.FindAsync(id);

            if (usersBasket == null)
            {
                return NotFound();
            }
            var user = await this.GetUserAsync();
            if (user.Id != usersBasket.UsedId && user.IsAdmin == false)
            {
                return Forbid();
            }

            return usersBasket;
        }

        // POST: api/UsersBasket
        [HttpPost]
        public async Task<ActionResult<UserBasket>> PostHistory(UserBasket usersBasket)
        {
            if (_context.UsersBasket.Any(t => t.UsedId == usersBasket.UsedId && t.ProductId == usersBasket.ProductId))
                return BadRequest();

            _context.UsersBasket.Add(usersBasket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistory", new { id = usersBasket.Id }, usersBasket);
        }

        // DELETE: api/UsersBasket/5
        [HttpDelete("{id}")]
        [HeaderValidator()]
        public async Task<IActionResult> DeleteBasket(int id)
        {
            var user = await this.GetUserAsync();
            var usersBasket = await _context.UsersBasket.FindAsync(id);
            if (usersBasket == null)
            {
                return NotFound();
            }
            if(user.IsAdmin == false)
                if(usersBasket.UsedId != user.Id)
                {
                    return NotFound();
                }

            _context.UsersBasket.Remove(usersBasket);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        // DELETE: api/UsersBasket/5
        [HttpDelete("ByProduct/{id}")]
        [HeaderValidator()]
        public async Task<IActionResult> DeleteBasketByProductId(int id)
        {
            var user = await this.GetUserAsync();
            var usersBasket = await _context.UsersBasket.FirstAsync(t => t.ProductId == id && t.UsedId == user.Id);
            if (usersBasket == null)
            {
                return NotFound();
            }
            _context.UsersBasket.Remove(usersBasket);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
