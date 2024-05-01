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
    public class RunsController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public RunsController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Run> currentList = _context.Runs
                .Include(x => x.PetReservations
                .Where(i => i.Reservation.StartDate <= DateTime.Today && i.Reservation.EndDate > DateTime.Today)
                )
                .ThenInclude(x => x.Reservation)
                .ToList();

            ViewData["VacancyRadio"] = TempData["VacancyRadio"] ?? "";
            ViewData["LargeCheck"] = TempData["LargeCheck"] ?? "";
            ViewData["RegCheck"] = TempData["RegCheck"] ?? "";
            ViewData["MultiplePetCheck"] = TempData["MultiplePetCheck"] ?? "";
            return View(currentList);
        }

        [HttpPost]
        public IActionResult Index(string VacancyRadio, string LargeCheck, string RegCheck, string MultiplePetCheck)
        {
            TempData["VacancyRadio"] = VacancyRadio;
            TempData["LargeCheck"] = LargeCheck;
            TempData["RegCheck"] = RegCheck;
            TempData["MultiplePetCheck"] = MultiplePetCheck;

            IEnumerable<Run> currentList = _context.Runs
                .Include(x => x.PetReservations
                .Where(i => i.Reservation.StartDate <= DateTime.Today && i.Reservation.EndDate > DateTime.Today)
                )
                .ThenInclude(x => x.Reservation)
                .ToList();

            if (VacancyRadio == "Vacant")
            {
                currentList = currentList.Where(x => x.Status == 1);
            }
            else
            {
                currentList = currentList.Where(x => x.Status == 2);
            }

            if ((!string.IsNullOrEmpty(LargeCheck)) && (!string.IsNullOrEmpty(RegCheck)))
            {
                currentList = currentList.Where(x => x.Size.ToUpper() == "L" || x.Size.ToUpper() == "R");
            }
            else
            {
                if (!string.IsNullOrEmpty(LargeCheck))
                {
                    currentList = currentList.Where(x => x.Size.ToUpper() == "L");
                }

                else if (!string.IsNullOrEmpty(RegCheck))
                {
                    currentList = currentList.Where(x => x.Size.ToUpper() == "R");
                }
            }

            if (!string.IsNullOrEmpty(MultiplePetCheck))
            {
                currentList = currentList.Where(x => x.PetReservations == null ? false : x.PetReservations.Count > 1);
            }

            ViewData["VacancyRadio"] = TempData["VacancyRadio"] ?? "";
            ViewData["LargeCheck"] = TempData["LargeCheck"] ?? "";
            ViewData["RegCheck"] = TempData["RegCheck"] ?? "";
            ViewData["MultiplePetCheck"] = TempData["MultiplePetCheck"] ?? "";
            return View(currentList);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id == null || _context.Runs == null)
            {
                return NotFound();
            }

            var run = _context.Runs
                .Include(x => x.PetReservations
                .Where(i => i.Reservation.StartDate <= DateTime.Today && i.Reservation.EndDate > DateTime.Today)
                )
                .ThenInclude(x => x.Pet)
                .FirstOrDefault(m => m.RunId == id);

            if (run == null)
            {
                return NotFound();
            }

            return View(run);
        }

        [HttpPost]
        public IActionResult Details(Run run)
        {
            int id = run.RunId;
            Run? findRun = _context.Runs
                .Include(x => x.PetReservations
                .Where(i => i.Reservation.StartDate <= DateTime.Today && i.Reservation.EndDate > DateTime.Today)
                )
                .ThenInclude(x => x.Reservation)
                .FirstOrDefault(x => x.RunId == id);

            if (findRun == null)
            {
                return NotFound();
            }

            run.PetReservations = findRun.PetReservations;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ChangeTracker.Clear();
                    _context.Update(run);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RunExists(run.RunId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction("Details", "Runs", new { id = id }); ;
        }

        [HttpPost]
        public IActionResult Unassign(int RunId, int PetResId)
        {
            Run? findRun = _context.Runs
                .Include(x => x.PetReservations
                .Where(i => i.Reservation.StartDate <= DateTime.Today && i.Reservation.EndDate > DateTime.Today)
                )
                .ThenInclude(x => x.Reservation)
                .FirstOrDefault(x => x.RunId == RunId);

            PetReservation? findPetRes = _context.PetReservations
                .Include(x => x.Reservation)
                .Where(x => x.Reservation.StartDate <= DateTime.Today && x.Reservation.EndDate > DateTime.Today)
                .FirstOrDefault(x => x.PetReservationId == PetResId);

            if (findRun == null || findPetRes == null)
            {
                return NotFound();
            }

            findPetRes.Run = null;
            findPetRes.RunId = null;
            findRun.PetReservations = findRun.PetReservations
                .Where(x => x.PetReservationId != PetResId).ToList();

            if (findRun.PetReservations.Count < 1)
            {
                findRun.Status = 3;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.ChangeTracker.Clear();
                    _context.Update(findPetRes);
                    _context.Update(findRun);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RunExists(findRun.RunId) || !PetReservationExists(findPetRes.PetReservationId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction("Details", "Runs", new { id = RunId }); ;
        }

        private bool RunExists(int id)
        {
            return (_context.Runs?.Any(e => e.RunId == id)).GetValueOrDefault();
        }

        private bool PetReservationExists(int id)
        {
            return (_context.PetReservations?.Any(e => e.PetReservationId == id)).GetValueOrDefault();
        }
    }
}
