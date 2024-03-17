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
    public class SendedCodesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SendedCodesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SendedCodes
        public async Task<IActionResult> Index()
        {
            return View(await _context.SendedCodes.ToListAsync());
        }

        // GET: SendedCodes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sendedCode = await _context.SendedCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sendedCode == null)
            {
                return NotFound();
            }

            return View(sendedCode);
        }

        // GET: SendedCodes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SendedCodes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Code,Limit")] SendedCode sendedCode)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sendedCode);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sendedCode);
        }

        // GET: SendedCodes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sendedCode = await _context.SendedCodes.FindAsync(id);
            if (sendedCode == null)
            {
                return NotFound();
            }
            return View(sendedCode);
        }

        // POST: SendedCodes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,Code,Limit")] SendedCode sendedCode)
        {
            if (id != sendedCode.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sendedCode);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SendedCodeExists(sendedCode.Id))
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
            return View(sendedCode);
        }

        // GET: SendedCodes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sendedCode = await _context.SendedCodes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sendedCode == null)
            {
                return NotFound();
            }

            return View(sendedCode);
        }

        // POST: SendedCodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sendedCode = await _context.SendedCodes.FindAsync(id);
            if (sendedCode != null)
            {
                _context.SendedCodes.Remove(sendedCode);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SendedCodeExists(int id)
        {
            return _context.SendedCodes.Any(e => e.Id == id);
        }
    }
}
