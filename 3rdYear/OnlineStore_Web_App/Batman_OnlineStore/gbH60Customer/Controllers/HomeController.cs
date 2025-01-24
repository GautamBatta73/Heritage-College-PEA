using gbH60Customer.DAL;
using gbH60Customer.Model;
using gbH60Customer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace gbH60Customer.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICustomerRepository _repo;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ICustomerRepository repo, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _repo = repo;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (HttpContext.Session.GetInt32("CustId") == null)
                {
                    Customer cust = await _repo.FindByEmail(User.Identity.Name);
                    HttpContext.Session.SetInt32("CustId", cust.CustomerId);
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
