using gbH60Services.DAL;
using gbH60Services.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _repo;

        public ShoppingCartController(IShoppingCartRepository repo)
        {
            _repo = repo;
        }

        public class CartRequest
        {
            public int CustId { get; set; }
            public int ProdId { get; set; }
        }

        [HttpGet("{custId}")]
        public ActionResult<ShoppingCart> GetCustomerCart(int custId)
        {
            var cart = _repo.FindByCustomer(custId);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        [HttpGet("GetItems/{custId}")]
        public IEnumerable<JoinProductCartItem> GetItemsByCustomer(int custId)
        {
            var cartItems = _repo.GetCartItems(custId);

            return cartItems;
        }

        [HttpPost("AddToCart")]
        public IActionResult AddToCart([FromBody] CartRequest req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.AddToCart(req.ProdId, req.CustId);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("RemoveFromCart")]
        public IActionResult RemoveFromCart([FromBody] CartRequest req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.RemoveFromCart(req.ProdId, req.CustId);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost("RemoveSingle")]
        public IActionResult RemoveSingle([FromBody] CartRequest req)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.RemoveSingle(req.ProdId, req.CustId);
                }
                catch (Exception e)
                {
                    return BadRequest(e.Message);
                }
                return NoContent();
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
