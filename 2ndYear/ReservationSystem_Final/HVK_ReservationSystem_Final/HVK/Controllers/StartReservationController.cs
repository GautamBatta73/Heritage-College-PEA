using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using HVK.Models;
using System.Reflection.Metadata.Ecma335;

namespace HVK.Controllers
{
    public class StartReservationController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public StartReservationController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return _context.Reservations != null ?
                View(await _context.Reservations.ToListAsync()) :
                Problem("Entity set 'HVKW24_Team7Context.Reservations'  is null.");
        }

        // GET: StartReservation/Create
        public async Task<IActionResult> Create()
        {
            int id = (HttpContext.Session.GetInt32("HvkUserID") ?? -1);
            var pets = await _context.Pets
                .Include(p => p.PetVaccinations)
                .ThenInclude(pv => pv.Vaccination)
                .Where(p => p.HvkuserId == id)
                .ToListAsync();
            var services = await _context.Services.ToListAsync();
            ViewBag.Pets = pets;
            ViewBag.Services = services;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation, int[] petIds, Dictionary<int, int[]> petServices)
        {
            if (petIds == null || petIds.Length == 0)
            {
                ModelState.AddModelError("", "At least one pet must be selected.");
            }
            if (petServices == null)
            {
                petServices = new Dictionary<int, int[]>();
            }

            if (!ModelState.IsValid)
            {
                int id = (HttpContext.Session.GetInt32("HvkUserID") ?? -1);
                ViewBag.Pets = await _context.Pets
                .Include(p => p.PetVaccinations)
                .ThenInclude(pv => pv.Vaccination)
                .Where(p => p.HvkuserId == id)
                .ToListAsync();
                ViewBag.Services = await _context.Services.ToListAsync();
                return View(reservation);
            }

            try
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                foreach (var petId in petIds)
                {
                    var petReservation = new PetReservation { PetId = petId, ReservationId = reservation.ReservationId };
                    _context.PetReservations.Add(petReservation);
                    await _context.SaveChangesAsync();

                    // Only attempt to add services if there are any associated with this pet
                    if (petServices.ContainsKey(petId) && petServices[petId] != null)
                    {
                        foreach (var serviceId in petServices[petId])
                        {
                            var petReservationService = new PetReservationService { PetReservationId = petReservation.PetReservationId, ServiceId = serviceId };
                            _context.PetReservationServices.Add(petReservationService);
                        }
                    }
                }

                await _context.SaveChangesAsync();

                return RedirectToAction("Success", "StartReservation");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request: " + ex.Message);
                return View(reservation);
            }
        }






        public async Task<IActionResult> Success()
        {
            return View();
    }
    }

    
}
