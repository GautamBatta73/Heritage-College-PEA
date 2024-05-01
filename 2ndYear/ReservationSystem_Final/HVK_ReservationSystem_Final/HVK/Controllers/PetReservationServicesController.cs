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
    public class PetReservationServicesController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public PetReservationServicesController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        // GET: PetReservationServices
        public async Task<IActionResult> Index()
        {
            var hVKW24_Team7Context = _context.PetReservationServices.Include(p => p.PetReservation).Include(p => p.Service);
            return View(await hVKW24_Team7Context.ToListAsync());
        }

        // GET: PetReservationServices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PetReservationServices == null)
            {
                return NotFound();
            }

            var petReservationService = await _context.PetReservationServices
                .Include(p => p.PetReservation)
                .Include(p => p.Service)
                .FirstOrDefaultAsync(m => m.PetReservationId == id);
            if (petReservationService == null)
            {
                return NotFound();
            }

            return View(petReservationService);
        }

        // GET: PetReservationServices/Create
        public IActionResult Create()
        {
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId");
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId");
            return View();
        }

        // POST: PetReservationServices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetReservationId,ServiceId,NullHelper")] PetReservationService petReservationService)
        {
            if (ModelState.IsValid)
            {
                _context.Add(petReservationService);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId", petReservationService.PetReservationId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", petReservationService.ServiceId);
            return View(petReservationService);
        }

        // GET: PetReservationServices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PetReservationServices == null)
            {
                return NotFound();
            }

            var petReservationService = await _context.PetReservationServices.FindAsync(id);
            if (petReservationService == null)
            {
                return NotFound();
            }
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId", petReservationService.PetReservationId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", petReservationService.ServiceId);
            return View(petReservationService);
        }

        // POST: PetReservationServices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PetReservationId,ServiceId,NullHelper")] PetReservationService petReservationService)
        {
            if (id != petReservationService.PetReservationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(petReservationService);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetReservationServiceExists(petReservationService.PetReservationId))
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
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId", petReservationService.PetReservationId);
            ViewData["ServiceId"] = new SelectList(_context.Services, "ServiceId", "ServiceId", petReservationService.ServiceId);
            return View(petReservationService);
        }

        // GET: PetReservationServices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PetReservationServices == null)
            {
                return NotFound();
            }

            var petReservationService = await _context.PetReservationServices
                .Include(p => p.PetReservation)
                .Include(p => p.Service)
                .FirstOrDefaultAsync(m => m.PetReservationId == id);
            if (petReservationService == null)
            {
                return NotFound();
            }

            return View(petReservationService);
        }

        // POST: PetReservationServices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PetReservationServices == null)
            {
                return Problem("Entity set 'HVKW24_Team7Context.PetReservationServices'  is null.");
            }
            var petReservationService = await _context.PetReservationServices.FindAsync(id);
            if (petReservationService != null)
            {
                _context.PetReservationServices.Remove(petReservationService);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetReservationServiceExists(int id)
        {
            return (_context.PetReservationServices?.Any(e => e.PetReservationId == id)).GetValueOrDefault();
        }
    }
}
