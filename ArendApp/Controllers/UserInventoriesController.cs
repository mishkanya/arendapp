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
    public class UserInventoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserInventoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserInventories/All
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<UserInventory>>> GetUsersInventoryAll()
        {
            return await _context.UsersInventory.ToListAsync();
        }

        // GET: api/UserInventories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInventory>>> GetUsersInventory()
        {
            var user = await this.GetUserAsync();
            return await _context.UsersInventory.Where(t => t.UsedId == user.Id).ToListAsync();
        }

        // GET: api/UserInventories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserInventory>> GetUserInventory(int id)
        {
            var userInventory = await _context.UsersInventory.FindAsync(id);


            if (userInventory == null)
            {
                return NotFound();
            }
            var user = await this.GetUserAsync();
            if (user.Id != userInventory.UsedId && user.IsAdmin == false)
            {
                return Forbid();
            }

            return userInventory;
        }

        // POST: api/UserInventories
        [HttpPost]
        public async Task<ActionResult<UserInventory>> PostUserInventory(UserInventory userInventory)
        {

            var user = await this.GetUserAsync();

            if(user.Id != userInventory.UsedId && user.IsAdmin == false )
            {
                return Forbid();
            }

            var product = await _context.Products.FindAsync(userInventory.ProductId);

            if (product == null)
            {
                return NotFound();
            }

            var basketProduct = await _context.UsersBasket.FirstOrDefaultAsync(t => t.ProductId == userInventory.ProductId && t.UsedId == userInventory.UsedId);
            if (basketProduct != null)
            {
                _context.Remove(basketProduct);
            }

            _context.UsersInventory.Add(userInventory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserInventory", new { id = userInventory.Id }, userInventory);
        }
        

        // DELETE: api/UserInventories/5
        [HttpDelete("{id}")]
        [HeaderValidator(true)]
        public async Task<IActionResult> DeleteUserInventory(int id)
        {
            var userInventory = await _context.UsersInventory.FindAsync(id);
            if (userInventory == null)
            {
                return NotFound();
            }

            _context.UsersInventory.Remove(userInventory);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
