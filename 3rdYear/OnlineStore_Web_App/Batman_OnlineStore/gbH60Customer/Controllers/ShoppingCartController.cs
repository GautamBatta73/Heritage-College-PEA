using gbH60Customer.DAL;
using gbH60Customer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace gbH60Customer.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _repo;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ICustomerRepository _custRepo;
        private readonly IOrderRepository _orderRepo;

        public ShoppingCartController(IShoppingCartRepository repo, SignInManager<IdentityUser> signInManager, ICustomerRepository custRepo, IOrderRepository orderRepo)
        {
            _repo = repo;
            _signInManager = signInManager;
            _custRepo = custRepo;
            _orderRepo = orderRepo;
        }

        public class CartRequest
        {
            public int CustId { get; set; }
            public int ProdId { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                int custId = (HttpContext.Session.GetInt32("CustId") ?? 0);
                if (custId != 0)
                {
                    ViewData["CustId"] = custId;
                    var cart = await _repo.GetCartItems(custId);

                    decimal subTotal = cart.Sum(x => x.CartItem.Price);
                    decimal taxes = subTotal * ((decimal)0.13);
                    decimal total = subTotal + taxes;

                    ViewData["SubTotal"] = subTotal;
                    ViewData["Taxes"] = taxes;
                    ViewData["Total"] = total;
                    return View(cart);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            if (_signInManager.IsSignedIn(User))
            {
                int custId = (HttpContext.Session.GetInt32("CustId") ?? 0);
                if (custId != 0)
                {
                    ViewData["CustId"] = custId;
                    var cust = await _custRepo.Find(custId);
                    var cart = await _repo.GetCartItems(custId);

                    if (cart.Any() && cust != null)
                    {
                        decimal subTotal = cart.Sum(x => x.CartItem.Price);
                        decimal taxes = subTotal * ((decimal)0.13);
                        decimal total = subTotal + taxes;

                        ViewData["SubTotal"] = subTotal;
                        ViewData["Taxes"] = taxes;
                        ViewData["Total"] = total;
                        ViewData["Cart"] = cart;
                        return View(new CheckoutCustomer(cust));
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutCustomer customer)
        {
            if (_signInManager.IsSignedIn(User))
            {
                int custId = (HttpContext.Session.GetInt32("CustId") ?? 0);
                if (custId != 0)
                {
                    ViewData["CustId"] = custId;
                    var cust = await _custRepo.Find(custId);
                    var cart = await _repo.GetCartItems(custId);

                    if (cart.Any() && cust != null)
                    {
                        string? cardCheck = await _orderRepo.CheckCreditCard(customer.CreditCard);
                        if (cardCheck != null)
                        {
                            ModelState.AddModelError("CreditCard", cardCheck);
                        }
                        if (!ModelState.IsValid)
                        {
                            decimal subTotal = cart.Sum(x => x.CartItem.Price);
                            decimal taxes = subTotal * ((decimal)0.13);
                            decimal total = subTotal + taxes;

                            ViewData["SubTotal"] = subTotal;
                            ViewData["Taxes"] = taxes;
                            ViewData["Total"] = total;
                            ViewData["Cart"] = cart;
                            return View(customer);
                        } else
                        {
                            await _orderRepo.Checkout(custId);
                            TempData["OrderPlaced"] = true;
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CartRequest request)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (request.CustId != 0)
                {
                    await _repo.AddToCart(request.ProdId, request.CustId);
                    return Ok();
                }
                else
                {
                    return BadRequest("No Id Registered");
                }
            }
            else
            {
                return BadRequest("Not Signed-In");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DelFromCart([FromBody] CartRequest request)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (request.CustId != 0)
                {
                    await _repo.RemoveFromCart(request.ProdId, request.CustId);
                    return Ok();
                }
                else
                {
                    return BadRequest("No Id Registered");
                }
            }
            else
            {
                return BadRequest("Not Signed-In");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DelSingleProduct([FromBody] CartRequest request)
        {
            if (_signInManager.IsSignedIn(User))
            {
                if (request.CustId != 0)
                {
                    await _repo.RemoveSingle(request.ProdId, request.CustId);
                    return Ok();
                }
                else
                {
                    return BadRequest("No Id Registered");
                }
            }
            else
            {
                return BadRequest("Not Signed-In");
            }
        }
    }
}
