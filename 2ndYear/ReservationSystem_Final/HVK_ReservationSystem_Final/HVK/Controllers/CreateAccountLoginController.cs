using HVK.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HVK.Controllers
{
    public class CreateAccountLoginController : Controller
    {

        private readonly HVKW24_Team7Context _context;

        public CreateAccountLoginController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("HvkuserId,FirstName,LastName,Email,Password,Street,City,Province,PostalCode,Phone,CellPhone,EmergencyContactFirstName,EmergencyContactLastName,EmergencyContactPhone,UserType")] Hvkuser hvkuser)
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
                        return RedirectToAction("Index", "Login");
                    }
                }
                else {
                    ModelState.AddModelError("", "Please Fill at LEAST 1 contact field.");
                }
            }
            return View(hvkuser);
        }
    }// end of class
}