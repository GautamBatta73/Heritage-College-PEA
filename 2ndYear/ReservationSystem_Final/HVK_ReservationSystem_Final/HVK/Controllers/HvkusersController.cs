using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HVK.Models;
using System.Reflection.Metadata.Ecma335;
using Newtonsoft.Json;

namespace HVK.Controllers
{
    public class HvkusersController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public HvkusersController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        // GET: Hvkusers
        public async Task<IActionResult> Index()
        {
            return _context.Hvkusers != null ?
                        View(await _context.Hvkusers.ToListAsync()) :
                        Problem("Entity set 'HVKW24_Team7Context.Hvkusers'  is null.");
        }



        // GET: Hvkusers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Hvkusers == null)
            {
                return NotFound();
            }

            var hvkuser = await _context.Hvkusers.Include(p => p.Pets)
               .FirstOrDefaultAsync(m => m.HvkuserId == id);

            if (hvkuser == null)
            {
                return NotFound();
            }

            return View(hvkuser);
        }

        // GET: Hvkusers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Hvkusers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HvkuserId,FirstName,LastName,Email,Password,Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone,UserType")] Hvkuser hvkuser)
        {
            if (ModelState.IsValid) {
                if (hvkuser.Email != null || hvkuser.Phone != null || hvkuser.CellPhone != null) {
                    if (hvkuser.UserType == "Customer" && (hvkuser.EmergencyContactFirstName == null || hvkuser.EmergencyContactLastName == null || hvkuser.EmergencyContactPhone == null))
                    {
                        ModelState.AddModelError("", "Please Fill All Your Missing Emergency Contact Information.");
                    }
                    else
                    {
                        _context.Add(hvkuser);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Index", "Customer");
                    }
                }
                else {
                    ModelState.AddModelError("", "Please Fill at LEAST 1 contact field.");
                }
            }
            return View(hvkuser);
        }

        // GET: Hvkusers/Edit/5
        public async Task<IActionResult> Edit()
        {
            int id = (HttpContext.Session.GetInt32("HvkUserID") ?? -1);

            if (id == null || _context.Hvkusers == null)
            {
                return RedirectToAction("Index", "HomePage");
            }

            var hvkuser = await _context.Hvkusers.FindAsync(id);
            if (hvkuser == null)
            {
                return RedirectToAction("Index", "HomePage");
            }

            return View(hvkuser);
        }


        // POST: Hvkusers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("HvkuserId,FirstName,LastName,Password,Email,Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone,UserType")] Hvkuser hvkuser)
        {
            int id = (HttpContext.Session.GetInt32("HvkUserID") ?? -1);
            if (id != hvkuser.HvkuserId)
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hvkuser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HvkuserExists(hvkuser.HvkuserId))
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "HomePage");
            }

            return View(hvkuser);
        }

        // GET: Hvkusers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Hvkusers == null)
            {
                return NotFound();
            }

            var hvkuser = await _context.Hvkusers
                .FirstOrDefaultAsync(m => m.HvkuserId == id);
            if (hvkuser == null)
            {
                return NotFound();
            }

            return View(hvkuser);
        }

        // POST: Hvkusers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Hvkusers == null)
            {
                return Problem("Entity set 'HVKW24_Team7Context.Hvkusers'  is null.");
            }
            var hvkuser = await _context.Hvkusers.FindAsync(id);
            if (hvkuser != null)
            {
                _context.Hvkusers.Remove(hvkuser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HvkuserExists(int id)
        {
            return (_context.Hvkusers?.Any(e => e.HvkuserId == id)).GetValueOrDefault();
        }
    }
}
