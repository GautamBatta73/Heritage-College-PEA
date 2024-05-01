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
    public class PetReservationsController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public PetReservationsController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        // GET: PetReservations
        public async Task<IActionResult> Index()
        {
            var hVKW24_Team7Context = _context.PetReservations.Include(p => p.Pet).Include(p => p.Reservation).Include(p => p.Run);
            return View(await hVKW24_Team7Context.ToListAsync());
        }

        // GET: PetReservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PetReservations == null)
            {
                return NotFound();
            }

            var petReservation = await _context.PetReservations
                .Include(p => p.Pet)
                .Include(p => p.Reservation)
                .Include(p => p.Run)
                .FirstOrDefaultAsync(m => m.PetReservationId == id);
            if (petReservation == null)
            {
                return NotFound();
            }

            return View(petReservation);
        }

        // GET: PetReservations/Create
        public IActionResult Create()
        {
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId");
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId");
            ViewData["RunId"] = new SelectList(_context.Runs, "RunId", "RunId");
            return View();
        }

        // POST: PetReservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetReservationId,PetId,ReservationId,RunId")] PetReservation petReservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(petReservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", petReservation.PetId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", petReservation.ReservationId);
            ViewData["RunId"] = new SelectList(_context.Runs, "RunId", "RunId", petReservation.RunId);
            return View(petReservation);
        }

        // GET: PetReservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PetReservations == null)
            {
                return NotFound();
            }

            var petReservation = await _context.PetReservations.FindAsync(id);
            if (petReservation == null)
            {
                return NotFound();
            }
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", petReservation.PetId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", petReservation.ReservationId);
            ViewData["RunId"] = new SelectList(_context.Runs, "RunId", "RunId", petReservation.RunId);
            return View(petReservation);
        }

        // POST: PetReservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PetReservationId,PetId,ReservationId,RunId")] PetReservation petReservation)
        {
            if (id != petReservation.PetReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petReservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetReservationExists(petReservation.PetReservationId))
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
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "PetId", petReservation.PetId);
            ViewData["ReservationId"] = new SelectList(_context.Reservations, "ReservationId", "ReservationId", petReservation.ReservationId);
            ViewData["RunId"] = new SelectList(_context.Runs, "RunId", "RunId", petReservation.RunId);
            return View(petReservation);
        }

        // GET: PetReservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PetReservations == null)
            {
                return NotFound();
            }

            var petReservation = await _context.PetReservations
                .Include(p => p.Pet)
                .Include(p => p.Reservation)
                .Include(p => p.Run)
                .FirstOrDefaultAsync(m => m.PetReservationId == id);
            if (petReservation == null)
            {
                return NotFound();
            }

            return View(petReservation);
        }

        // POST: PetReservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PetReservations == null)
            {
                return Problem("Entity set 'HVKW24_Team7Context.PetReservations'  is null.");
            }
            var petReservation = await _context.PetReservations.FindAsync(id);
            if (petReservation != null)
            {
                _context.PetReservations.Remove(petReservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetReservationExists(int id)
        {
            return (_context.PetReservations?.Any(e => e.PetReservationId == id)).GetValueOrDefault();
        }
    }
}
