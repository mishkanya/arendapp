using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ArendApp.Api.Services;
using ArendApp.Models;

namespace ArendApp.Api.Controllers
{
    public class UserInventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserInventoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserInventories
        public async Task<IActionResult> Index()
        {
            return View(await _context.UsersInventory.ToListAsync());
        }

        // GET: UserInventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInventory = await _context.UsersInventory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInventory == null)
            {
                return NotFound();
            }

            return View(userInventory);
        }

        // GET: UserInventories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserInventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StartPeriod,EndPeriod,Id,UsedId,ProductId")] UserInventory userInventory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userInventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userInventory);
        }

        // GET: UserInventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInventory = await _context.UsersInventory.FindAsync(id);
            if (userInventory == null)
            {
                return NotFound();
            }
            return View(userInventory);
        }

        // POST: UserInventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StartPeriod,EndPeriod,Id,UsedId,ProductId")] UserInventory userInventory)
        {
            if (id != userInventory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userInventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserInventoryExists(userInventory.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userInventory);
        }

        // GET: UserInventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userInventory = await _context.UsersInventory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userInventory == null)
            {
                return NotFound();
            }

            return View(userInventory);
        }

        // POST: UserInventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userInventory = await _context.UsersInventory.FindAsync(id);
            if (userInventory != null)
            {
                _context.UsersInventory.Remove(userInventory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserInventoryExists(int id)
        {
            return _context.UsersInventory.Any(e => e.Id == id);
        }
    }
}
