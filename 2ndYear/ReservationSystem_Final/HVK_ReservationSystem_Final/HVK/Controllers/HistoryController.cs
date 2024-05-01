using HVK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HVK.Controllers
{
    public class HistoryController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public HistoryController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int id = (HttpContext.Session.GetInt32("HvkUserID") ?? -1);
            var customer = await _context.Hvkusers.FirstOrDefaultAsync(u => u.HvkuserId == id);
            if (customer == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var pets = await _context.Pets.Where(p => p.HvkuserId == id).ToListAsync();

            var reservations = await _context.Reservations
                .Where(r => r.PetReservations.Any(pr => pr.Pet.HvkuserId == id && pr.Reservation.StartDate < DateTime.Today))
                .Include(r => r.PetReservations)
                .ThenInclude(pr => pr.Pet)
                .ToListAsync();

            var viewModel = new JoinCustomerReservationsPets
            {
                Hvkuser = customer,
                Pets = pets,
                Reservations = reservations
            };

            return View(viewModel);
        }
    }
}
