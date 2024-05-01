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
    public class PetReservationDiscountsController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public PetReservationDiscountsController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        // GET: PetReservationDiscounts
        public async Task<IActionResult> Index()
        {
            var hVKW24_Team7Context = _context.PetReservationDiscounts.Include(p => p.Discount).Include(p => p.PetReservation);
            return View(await hVKW24_Team7Context.ToListAsync());
        }

        // GET: PetReservationDiscounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PetReservationDiscounts == null)
            {
                return NotFound();
            }

            var petReservationDiscount = await _context.PetReservationDiscounts
                .Include(p => p.Discount)
                .Include(p => p.PetReservation)
                .FirstOrDefaultAsync(m => m.DiscountId == id);
            if (petReservationDiscount == null)
            {
                return NotFound();
            }

            return View(petReservationDiscount);
        }

        // GET: PetReservationDiscounts/Create
        public IActionResult Create()
        {
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountId");
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId");
            return View();
        }

        // POST: PetReservationDiscounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscountId,PetReservationId,NullHelper")] PetReservationDiscount petReservationDiscount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(petReservationDiscount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountId", petReservationDiscount.DiscountId);
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId", petReservationDiscount.PetReservationId);
            return View(petReservationDiscount);
        }

        // GET: PetReservationDiscounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PetReservationDiscounts == null)
            {
                return NotFound();
            }

            var petReservationDiscount = await _context.PetReservationDiscounts.FindAsync(id);
            if (petReservationDiscount == null)
            {
                return NotFound();
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountId", petReservationDiscount.DiscountId);
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId", petReservationDiscount.PetReservationId);
            return View(petReservationDiscount);
        }

        // POST: PetReservationDiscounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscountId,PetReservationId,NullHelper")] PetReservationDiscount petReservationDiscount)
        {
            if (id != petReservationDiscount.DiscountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petReservationDiscount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetReservationDiscountExists(petReservationDiscount.DiscountId))
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
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountId", petReservationDiscount.DiscountId);
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId", petReservationDiscount.PetReservationId);
            return View(petReservationDiscount);
        }

        // GET: PetReservationDiscounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PetReservationDiscounts == null)
            {
                return NotFound();
            }

            var petReservationDiscount = await _context.PetReservationDiscounts
                .Include(p => p.Discount)
                .Include(p => p.PetReservation)
                .FirstOrDefaultAsync(m => m.DiscountId == id);
            if (petReservationDiscount == null)
            {
                return NotFound();
            }

            return View(petReservationDiscount);
        }

        // POST: PetReservationDiscounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PetReservationDiscounts == null)
            {
                return Problem("Entity set 'HVKW24_Team7Context.PetReservationDiscounts'  is null.");
            }
            var petReservationDiscount = await _context.PetReservationDiscounts.FindAsync(id);
            if (petReservationDiscount != null)
            {
                _context.PetReservationDiscounts.Remove(petReservationDiscount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetReservationDiscountExists(int id)
        {
            return (_context.PetReservationDiscounts?.Any(e => e.DiscountId == id)).GetValueOrDefault();
        }
    }
}
