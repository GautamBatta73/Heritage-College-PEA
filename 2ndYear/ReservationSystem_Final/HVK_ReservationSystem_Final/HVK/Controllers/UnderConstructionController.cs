using HVK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HVK.Controllers
{
    public class UnderConstructionController : Controller
    {
        private readonly HVKW24_Team7Context _context;

        public UnderConstructionController(HVKW24_Team7Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            string userString = HttpContext.Session.GetString("HvkUserObject");
            Hvkuser? user = userString == null ? null : JsonConvert.DeserializeObject<Hvkuser>(userString);

            if (user == null)
            {
                return View();
            }
            else if (user.UserType == "Customer")
                return View();
            else
                return RedirectToAction("IndexEmp", "UnderConstruction");
        }

        public IActionResult IndexEmp()
        {
            return View();
        }
    }
}
