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
    public class MedicationsController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public MedicationsController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        // GET: Medications
        public async Task<IActionResult> Index()
        {
            var hVKW24_Team7Context = _context.Medications.Include(m => m.PetReservation);
            return View(await hVKW24_Team7Context.ToListAsync());
        }

        // GET: Medications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications
                .Include(m => m.PetReservation)
                .FirstOrDefaultAsync(m => m.MedicationId == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // GET: Medications/Create
        public IActionResult Create()
        {
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId");
            return View();
        }

        // POST: Medications/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MedicationId,Name,Dosage,SpecialInstruct,EndDate,PetReservationId")] Medication medication)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId", medication.PetReservationId);
            return View(medication);
        }

        // GET: Medications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications.FindAsync(id);
            if (medication == null)
            {
                return NotFound();
            }
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId", medication.PetReservationId);
            return View(medication);
        }

        // POST: Medications/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MedicationId,Name,Dosage,SpecialInstruct,EndDate,PetReservationId")] Medication medication)
        {
            if (id != medication.MedicationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicationExists(medication.MedicationId))
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
            ViewData["PetReservationId"] = new SelectList(_context.PetReservations, "PetReservationId", "PetReservationId", medication.PetReservationId);
            return View(medication);
        }

        // GET: Medications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Medications == null)
            {
                return NotFound();
            }

            var medication = await _context.Medications
                .Include(m => m.PetReservation)
                .FirstOrDefaultAsync(m => m.MedicationId == id);
            if (medication == null)
            {
                return NotFound();
            }

            return View(medication);
        }

        // POST: Medications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Medications == null)
            {
                return Problem("Entity set 'HVKW24_Team7Context.Medications'  is null.");
            }
            var medication = await _context.Medications.FindAsync(id);
            if (medication != null)
            {
                _context.Medications.Remove(medication);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicationExists(int id)
        {
            return (_context.Medications?.Any(e => e.MedicationId == id)).GetValueOrDefault();
        }
    }
}
