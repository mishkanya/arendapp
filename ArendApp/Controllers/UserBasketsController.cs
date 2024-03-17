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
    public class UserBasketsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserBasketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserBaskets
        public async Task<IActionResult> Index()
        {
            return View(await _context.UsersBasket.ToListAsync());
        }

        // GET: UserBaskets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBasket = await _context.UsersBasket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBasket == null)
            {
                return NotFound();
            }

            return View(userBasket);
        }

        // GET: UserBaskets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserBaskets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsedId,ProductId")] UserBasket userBasket)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userBasket);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userBasket);
        }

        // GET: UserBaskets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBasket = await _context.UsersBasket.FindAsync(id);
            if (userBasket == null)
            {
                return NotFound();
            }
            return View(userBasket);
        }

        // POST: UserBaskets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsedId,ProductId")] UserBasket userBasket)
        {
            if (id != userBasket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userBasket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserBasketExists(userBasket.Id))
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
            return View(userBasket);
        }

        // GET: UserBaskets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userBasket = await _context.UsersBasket
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userBasket == null)
            {
                return NotFound();
            }

            return View(userBasket);
        }

        // POST: UserBaskets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userBasket = await _context.UsersBasket.FindAsync(id);
            if (userBasket != null)
            {
                _context.UsersBasket.Remove(userBasket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserBasketExists(int id)
        {
            return _context.UsersBasket.Any(e => e.Id == id);
        }
    }
}
