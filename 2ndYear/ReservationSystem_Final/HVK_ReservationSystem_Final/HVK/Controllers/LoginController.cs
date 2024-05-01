using HVK.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace HVK.Controllers
{
    public class LoginController : Controller
    {

        private readonly HVKW24_Team7Context _context;

        public LoginController(HVKW24_Team7Context context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetInt32("HvkUserID", -1);
            HttpContext.Session.SetString("HvkUserObject", "");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Login model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Hvkusers
                    .Where(x => x.Email == model.UserEmail && x.Password == model.UserPassword)
                    .FirstOrDefaultAsync();

                if (user == null) {
                    ModelState.AddModelError("invalidcreds", "This Email and Password Combination is Incorrect Please Try Again.");
                }
                else if (user.UserType == "Customer" && (user.EmergencyContactFirstName == null || user.EmergencyContactLastName == null || user.EmergencyContactPhone == null)) {
                    ModelState.AddModelError("", "Please Update Your Missing Emergency Contact Information.");
                }
                else {
                    // Could use session to hold this value instead of assingning it in the ViewData.
                    // Roles could aslo be used for this although they require a bit of work to set up but works great.

                    Hvkuser hvkuser = new Hvkuser
                    {
                        HvkuserId = user.HvkuserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Password = user.Password,
                        Street = user.Street,
                        City = user.City,
                        Province = user.Province,
                        PostalCode = user.PostalCode,
                        Phone = user.Phone,
                        CellPhone = user.CellPhone,
                        EmergencyContactFirstName = user.EmergencyContactFirstName,
                        EmergencyContactLastName = user.EmergencyContactLastName,
                        EmergencyContactPhone = user.EmergencyContactPhone,
                        UserType = user.UserType,
                        Pets = user.Pets // Assuming you also want to copy the pets
                    };

                    // just storing the user id for simple use, if you dont want to access an entire object
                    HttpContext.Session.SetInt32("HvkUserID", hvkuser.HvkuserId);

                    //converting HvkUser obj to Json obj and storing it in session
                    string HVKUserString = JsonConvert.SerializeObject(hvkuser);
                    HttpContext.Session.SetString("HvkUserObject", HVKUserString);

                    // How to access the session info
                    string userString = HttpContext.Session.GetString("HvkUserObject");
                    Hvkuser userObj = JsonConvert.DeserializeObject<Hvkuser>(userString);

                    if (user.UserType == "Customer")
                        return RedirectToAction("Index", "HomePage");
                    else
                        return RedirectToAction("Staff", "HomePage");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
    }
}
