using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HVK.Models;

namespace HVK.Views.PetVaccinations
{
    public class EditModel : PageModel
    {
        private readonly HVKW24_Team7Context _context;

        public EditModel(HVKW24_Team7Context context)
        {
            _context = context;
        }

        [BindProperty]
        public PetVaccination PetVaccination { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PetVaccinations == null)
            {
                return NotFound();
            }

            var petvaccination = await _context.PetVaccinations.FirstOrDefaultAsync(m => m.VaccinationId == id);
            if (petvaccination == null)
            {
                return NotFound();
            }
            PetVaccination = petvaccination;
            ViewData["PetId"] = new SelectList(_context.Pets, "PetId", "Gender");
            ViewData["VaccinationId"] = new SelectList(_context.Vaccinations, "VaccinationId", "VaccinationId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(PetVaccination).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PetVaccinationExists(PetVaccination.VaccinationId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PetVaccinationExists(int id)
        {
            return (_context.PetVaccinations?.Any(e => e.VaccinationId == id)).GetValueOrDefault();
        }
    }
}
