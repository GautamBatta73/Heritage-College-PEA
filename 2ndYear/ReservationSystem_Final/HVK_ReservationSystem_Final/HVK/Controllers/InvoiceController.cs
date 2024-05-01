using HVK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HVK.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public InvoiceController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            if (InvoiceExists(id))
            {
                var ReservationInvoice = _context.PetReservations.Include(x => x.Reservation)
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
                ViewBag.ResInvoice = ReservationInvoice;
                ViewBag.ResDiscounts = ReservationDiscounts;
                ViewBag.ResRates = ReservationRates;

                return View();
            } else
            {
                return NotFound();
            }
        }
        private bool InvoiceExists(int id)
        {
            return (_context.Reservations?.Any(x => x.ReservationId == id)).GetValueOrDefault();
        }

        [HttpPost]
        public async Task<ActionResult> Index()
        {
            int resID = Convert.ToInt32(ViewBag.ReservationID ?? -999);

            var currentReservation = _context.Reservations
                                    .Where(x => x.ReservationId == resID)
                                    .FirstOrDefault();

            currentReservation.Status = 5;

            try
            {
                _context.Update(currentReservation);
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(currentReservation.ReservationId))
                {
                    return NotFound();
                } else
                {
                    throw;
                }
            }

            return RedirectToAction("Staff", "HomePage");       
        }
       
    }
}
