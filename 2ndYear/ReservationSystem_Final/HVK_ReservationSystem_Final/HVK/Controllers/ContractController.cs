using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HVK.Models;
using Humanizer.Localisation.TimeToClockNotation;

namespace HVK.Controllers
{
    public class ContractController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public ContractController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            if (ReservationExists(id))
            {
                var ReservationContract = _context.PetReservations.Include(x => x.Reservation)
                    .Include(x => x.Pet)
                    .ThenInclude(x => x.Hvkuser)
                    .Include(x => x.PetReservationServices)
                    .Include(x => x.PetReservationDiscounts)
                    .ThenInclude(x => x.Discount)
                    .Where(x => x.ReservationId == id)
                    .ToListAsync();

                var ReservationRates = _context.DailyRates.ToList();
                var ReservationDiscounts = _context.ReservationDiscounts.Include(x => x.Discount)
                    .Where(x => x.ReservationId == id)
                    .FirstOrDefault();

                // Pass the reservation infromation

                ViewBag.ReservationID = id;
                ViewBag.ResContract = ReservationContract;
                ViewBag.ResDiscounts = ReservationDiscounts;
                ViewBag.ResRates = ReservationRates;
                
                return View();
            } else
            {
                return NotFound();
            }

        }

        private bool ReservationExists(int id)
        {
            return (_context.Reservations?.Any(r => r.ReservationId == id)).GetValueOrDefault();
        }
    }
}
