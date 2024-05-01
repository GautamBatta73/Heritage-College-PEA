using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HVK.Models;

namespace HVK.Controllers
{
    public class DailyRatesController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public DailyRatesController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        // GET: DailyRates
        public async Task<IActionResult> Index()
        {
            var hVKW24_Team7Context = _context.DailyRates.Include(d => d.Service);
            return View(await hVKW24_Team7Context.ToListAsync());
        }

        // GET: DailyRates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DailyRates == null)
            {
                return NotFound();
            }

            var dailyRate = await _context.DailyRates
                .Include(d => d.Service)
                .FirstOrDefaultAsync(m => m.DailyRateId == id);
            if (dailyRate == null)
            {
                return NotFound();
            }

            return View(dailyRate);
        }

        // GET: DailyRates/Create
        public IActionResult Create()
        {
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId");
            return View();
        }

        // POST: DailyRates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DailyRateId,Rate,DogSize,ServiceId")] DailyRate dailyRate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailyRate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", dailyRate.ServiceId);
            return View(dailyRate);
        }

        // GET: DailyRates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DailyRates == null)
            {
                return NotFound();
            }

            var dailyRate = await _context.DailyRates.FindAsync(id);
            if (dailyRate == null)
            {
                return NotFound();
            }
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", dailyRate.ServiceId);
            return View(dailyRate);
        }

        // POST: DailyRates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DailyRateId,Rate,DogSize,ServiceId")] DailyRate dailyRate)
        {
            if (id != dailyRate.DailyRateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailyRate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailyRateExists(dailyRate.DailyRateId))
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
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", dailyRate.ServiceId);
            return View(dailyRate);
        }

        // GET: DailyRates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DailyRates == null)
            {
                return NotFound();
            }

            var dailyRate = await _context.DailyRates
                .Include(d => d.Service)
                .FirstOrDefaultAsync(m => m.DailyRateId == id);
            if (dailyRate == null)
            {
                return NotFound();
            }

            return View(dailyRate);
        }

        // POST: DailyRates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DailyRates == null)
            {
                return Problem("Entity set 'HVKW24_Team7Context.DailyRates'  is null.");
            }
            var dailyRate = await _context.DailyRates.FindAsync(id);
            if (dailyRate != null)
            {
                _context.DailyRates.Remove(dailyRate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailyRateExists(int id)
        {
            return (_context.DailyRates?.Any(e => e.DailyRateId == id)).GetValueOrDefault();
        }
    }
}
