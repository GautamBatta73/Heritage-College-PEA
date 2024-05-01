using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HVK.Models;
using Microsoft.CodeAnalysis;
using System.Net.NetworkInformation;

using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Humanizer;

namespace HVK.Controllers
{
    public class ReservationsController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public ReservationsController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            return _context.Reservations != null ?
                        View(await _context.Reservations.ToListAsync()) :
                        Problem("Entity set 'HVKW24_Team7Context.Reservations'  is null.");
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Retrieve user session info
            string userString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);

            // Pass userObj to the view
            ViewData["HVKUserObj"] = userObj;

            return View(reservation);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReservationId,StartDate,EndDate,Status")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservations == null) {
                return NotFound();
            }

            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Retrieve user session info
            string userString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);

            // Pass userObj to the view
            ViewData["HVKUserObj"] = userObj;

            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("ReservationId,StartDate,EndDate,Status")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationExists(reservation.ReservationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Retrieve user session info
                string userString = HttpContext.Session.GetString("HvkUserObject");
                Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);

                if (reservation.Status == 5) {
                    return RedirectToAction("Index", "Home");
                }
                else if (userObj.UserType == "Employee") {
                    return RedirectToAction("Staff", "HomePage");
                }
                else {
                    return RedirectToAction("Index", "HomePage");
                }
            }

            // Retrieve user session info
            string viewUserString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser viewUserObj = JsonConvert.DeserializeObject<Hvkuser>(viewUserString);
            // Pass userObj to the view
            ViewData["HVKUserObj"] = viewUserObj;

            return View(reservation);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservations == null)
            {
                return NotFound();
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(m => m.ReservationId == id);
            if (reservation == null)
            {
                return NotFound();
            }

            // Retrieve user session info
            string userString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);

            // Pass userObj to the view
            ViewData["HVKUserObj"] = userObj;

            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservations == null)
            {
                return Problem("Entity set 'HVKW24_Team7Context.Reservations'  is null.");
            }
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationExists(int id)
        {
            return (_context.Reservations?.Any(e => e.ReservationId == id)).GetValueOrDefault();
        }
    }
}
