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
    public class ReservationDiscountsController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public ReservationDiscountsController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        // GET: ReservationDiscounts
        public async Task<IActionResult> Index()
        {
            var hVKW24_Team7Context = _context.ReservationDiscounts.Include(r => r.Discount).Include(r => r.Reservation);
            return View(await hVKW24_Team7Context.ToListAsync());
        }

        // GET: ReservationDiscounts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ReservationDiscounts == null)
            {
                return NotFound();
            }

            var reservationDiscount = await _context.ReservationDiscounts
                .Include(r => r.Discount)
                .Include(r => r.Reservation)
                .FirstOrDefaultAsync(m => m.DiscountId == id);
            if (reservationDiscount == null)
            {
                return NotFound();
            }

            return View(reservationDiscount);
        }

        // GET: ReservationDiscounts/Create
        public IActionResult Create()
        {
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountId");
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId");
            return View();
        }

        // POST: ReservationDiscounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiscountId,ReservationId,NullHelper")] ReservationDiscount reservationDiscount)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservationDiscount);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountId", reservationDiscount.DiscountId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", reservationDiscount.ReservationId);
            return View(reservationDiscount);
        }

        // GET: ReservationDiscounts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ReservationDiscounts == null)
            {
                return NotFound();
            }

            var reservationDiscount = await _context.ReservationDiscounts.FindAsync(id);
            if (reservationDiscount == null)
            {
                return NotFound();
            }
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountId", reservationDiscount.DiscountId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", reservationDiscount.ReservationId);
            return View(reservationDiscount);
        }

        // POST: ReservationDiscounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiscountId,ReservationId,NullHelper")] ReservationDiscount reservationDiscount)
        {
            if (id != reservationDiscount.DiscountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservationDiscount);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationDiscountExists(reservationDiscount.DiscountId))
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
            ViewData["DiscountId"] = new SelectList(_context.Discounts, "DiscountId", "DiscountId", reservationDiscount.DiscountId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", reservationDiscount.ReservationId);
            return View(reservationDiscount);
        }

        // GET: ReservationDiscounts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ReservationDiscounts == null)
            {
                return NotFound();
            }

            var reservationDiscount = await _context.ReservationDiscounts
                .Include(r => r.Discount)
                .Include(r => r.Reservation)
                .FirstOrDefaultAsync(m => m.DiscountId == id);
            if (reservationDiscount == null)
            {
                return NotFound();
            }

            return View(reservationDiscount);
        }

        // POST: ReservationDiscounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ReservationDiscounts == null)
            {
                return Problem("Entity set 'HVKW24_Team7Context.ReservationDiscounts'  is null.");
            }
            var reservationDiscount = await _context.ReservationDiscounts.FindAsync(id);
            if (reservationDiscount != null)
            {
                _context.ReservationDiscounts.Remove(reservationDiscount);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationDiscountExists(int id)
        {
            return (_context.ReservationDiscounts?.Any(e => e.DiscountId == id)).GetValueOrDefault();
        }
    }
}
