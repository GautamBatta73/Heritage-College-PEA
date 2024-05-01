using HVK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;

namespace HVK.Controllers
{
    public class CustomerController : Controller
    {
        public readonly HVKW24_Team7Context _context;

        public CustomerController(HVKW24_Team7Context context)
        {
            _context = context;
        }


        public IActionResult Index(string searchString)
        {
            ViewData["Search"] = searchString ?? "";
            if (!String.IsNullOrEmpty(searchString))
            {
                var customers = _context.Hvkusers
                    .Include(x => x.Pets)
                    .Where(s => s.UserType == "Customer" && (s.FirstName + " " + s.LastName)
                    .Contains(searchString)).OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName).ToList();

                if (customers.Count > 0)
                {
                    return View(customers);
                }
                else
                {
                    ViewData["ErrorMessage"] = "Can't find user, try again!";
                }
            }
            return View();
        }



        public async Task<IActionResult> Customer(int id)
        {
            var customer = await _context.Hvkusers.FirstOrDefaultAsync(u => u.HvkuserId == id);
            if (customer == null)
            {
                return NotFound();
            }

            var pets = await _context.Pets.Where(p => p.HvkuserId == id).ToListAsync();

            var reservations = await _context.Reservations
                .Where(r => r.PetReservations.Any(pr => pr.Pet.HvkuserId == id))
                .Include(r => r.PetReservations)
                .ThenInclude(pr => pr.Pet)
                .ToListAsync();

            var viewModel = new JoinCustomerReservationsPets
            {
                Hvkuser = customer,
                Pets = pets,
                Reservations = reservations
            };

            string userString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);

            // Pass userObj to the view
            ViewData["HVKUserObj"] = userObj;

            return View(viewModel);
        }


    }
}
