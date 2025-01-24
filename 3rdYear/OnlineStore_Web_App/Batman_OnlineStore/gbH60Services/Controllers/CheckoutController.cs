using gbH60Services.DAL;
using Microsoft.AspNetCore.Mvc;
using ServiceReference1;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace gbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IShoppingCartRepository _cartRepo;
        private readonly IOrderRepository _repo;

        public CheckoutController(IShoppingCartRepository cartRepo, IOrderRepository repo)
        {
            _cartRepo = cartRepo;
            _repo = repo;
        }

        public class CheckoutRequest
        {
            public int CustId { get; set; }
        }

        [HttpPost]
        public IActionResult Checkout([FromBody] CheckoutRequest cust)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repo.Checkout(cust.CustId);
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

        [HttpGet("CheckCreditCard")]
        //GoodCard = 0, ErrInvalidLength = -1, ErrNotAllNum = -2, ErrCheckSum = -3, ErrProduct = -4 , ErrBalance = -5
        public async Task<IActionResult> CheckCard(string? cardNum)
        {
            var client = new CheckCreditCardSoapClient(CheckCreditCardSoapClient.EndpointConfiguration.CheckCreditCardSoap);
            var result = await client.CreditCardCheckAsync(cardNum ?? "");

            if (result == 0)
            {
                return Ok();
            }
            else
            {
                string message = "Unexpected Error Occured.";

                switch (result)
                {
                    case -1:
                        message = "Card Number must between 12 and 16 characters.";
                        break;

                    case -2:
                        message = "Card Number must all be numbers.";
                        break;

                    case -3:
                        message = "Card Number's 4 sections must sum to less than 30.";
                        break;

                    case -4:
                        message = "Last 2 digit must multiply to an even number.";
                        break;

                    case -5:
                        message = "Insufficient Balance.";
                        break;
                }

                return BadRequest(message);
            }
        }
    }
}
