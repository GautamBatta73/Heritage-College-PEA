using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HVK.Models;
using Newtonsoft.Json;

namespace HVK.Controllers
{
    public class PetsController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public PetsController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        // GET: Pets
        public async Task<IActionResult> Index()
        {
            int id = (HttpContext.Session.GetInt32("HvkUserID") ?? -1);

            string userString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);

            if (userObj.UserType == "Customer" && userObj.HvkuserId != null)
            {
                if (id == null || _context.Pets == null)
                {
                    return NotFound();
                }

                var pet = await _context.Pets
                          .Where(p => p.HvkuserId == id)
                          .ToListAsync();

                if (pet == null)
                {
                    return NotFound();
                }

                return View(pet);
            } else
            {
                var petsList = await _context.Pets.Include(p => p.Hvkuser).ToListAsync();
                return View(petsList);
            }
        }

        // GET: Pets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            string userString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);

            if (userObj.UserType == "Employee")
            {
                if (id == null || _context.Pets == null)
                {
                    return NotFound();
                }

                var pet = await _context.Pets
                    .Include(p => p.Hvkuser)
                    .Include(p => p.PetVaccinations)
                    .ThenInclude(p => p.Vaccination)
                    .FirstOrDefaultAsync(m => m.PetId == id);

                if (pet == null)
                {
                    return NotFound();
                }

                ViewData["HVKUserObj"] = userObj;
                return View(pet);
            } else
            {
                if (id == null || _context.Pets == null)
                {
                    return NotFound();
                }

                var pet = await _context.Pets
                    .Include(p => p.Hvkuser)
                    .Include(p => p.PetVaccinations)
                    .ThenInclude(p => p.Vaccination)
                    .FirstOrDefaultAsync(m => m.PetId == id);

                if (pet == null)
                {
                    return RedirectToAction("Details", "HVKUser");
                }

                ViewData["HVKUserObj"] = userObj;
                return View(pet);
            }
        }

        // GET: Pets/Create
        public IActionResult Create()
        {
            ViewData["HvkuserId"] = new SelectList(_context.Hvkusers, "HvkuserId", "HvkuserId");
            return View();
        }

        // POST: Pets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PetId,Name,Gender,Breed,Birthyear,HvkuserId,DogSize,Climber,Barker,SpecialNotes,Sterilized")] Pet pet)
        {
            ModelState.Remove("Hvkuser");
            if (ModelState.IsValid)
            {
                string userString = HttpContext.Session.GetString("HvkUserObject");
                Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);
                try
                {
                    pet.HvkuserId = userObj.HvkuserId;
                    _context.Add(pet);
                    await _context.SaveChangesAsync();

                    // Bind the pet to the corresponding hvk user
                    var hvkusers = await _context.Hvkusers.Include(p => p.Pets)
                                .FirstOrDefaultAsync(m => m.HvkuserId == userObj.HvkuserId);

                    if (userObj.UserType == "Employee")
                    {
                        return RedirectToAction("Staff", "HomePage");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Pets");
                    }
                } catch
                {
                    return RedirectToAction("Staff", "HomePage");

                }
            }
            // OLDER ONE -> ViewData["HvkuserId"] = new SelectList(_context.Hvkusers, "HvkuserId", "HvkuserId", pet.HvkuserId);


            return View(pet);
        }

        // GET: Pets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets.FindAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            // OLDER ONE -> ViewData["HvkuserId"] = new SelectList(_context.Hvkusers, "HvkuserId", "HvkuserId", pet.HvkuserId);

            // Retrieve user session info
            string userString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);

            // Pass userObj to the view
            ViewData["HVKUserObj"] = userObj;

            return View(pet);
        }

        // POST: Pets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("PetId,Name,Gender,Breed,Birthyear,HvkuserId,DogSize,Climber,Barker,SpecialNotes,Sterilized")] Pet pet)
        {
            ModelState.Remove("Hvkuser");
            if (ModelState.IsValid)
            {
                // Retrieve user session info
                string userString = HttpContext.Session.GetString("HvkUserObject");
                Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);
                try
                {
                    pet.HvkuserId = userObj.HvkuserId;
                    _context.Update(pet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PetExists(pet.PetId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                if (userObj.UserType == "Employee")
                {
                    return RedirectToAction("Staff", "HomePage");
                }
                else
                {
                    return RedirectToAction("Index", "HomePage");
                }
            }

            // Retrieve user session info
            string viewUserString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser viewUserObj = JsonConvert.DeserializeObject<Hvkuser>(viewUserString);

            // Pass userObj to the view
            ViewData["HVKUserObj"] = viewUserObj;

            return View(pet);
        }

        // GET: Pets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pets == null)
            {
                return NotFound();
            }

            var pet = await _context.Pets
                .Include(p => p.Hvkuser)
                .FirstOrDefaultAsync(m => m.PetId == id);
            if (pet == null)
            {
                return NotFound();
            }

            // Retrieve user session info
            string userString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);

            // Pass userObj to the view
            ViewData["HVKUserObj"] = userObj;

            return View(pet);
        }

        // POST: Pets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pets == null)
            {
                return Problem("Entity set 'HVKW24_Team7Context.Pets'  is null.");
            }
            var pet = await _context.Pets.FindAsync(id);
            if (pet != null)
            {
                _context.Pets.Remove(pet);
            }

            // Retrieve user session info
            string userString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);

            // Pass userObj to the view
            ViewData["HVKUserObj"] = userObj;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PetExists(int id)
        {
            return (_context.Pets?.Any(e => e.PetId == id)).GetValueOrDefault();
        }
    }
}
