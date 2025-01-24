using gbH60Services.DAL;
using gbH60Services.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _repo;

        public CustomersController(ICustomerRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<Customer> GetCustomers()
        {
            return _repo.GetAll();
        }

        [HttpGet("email/{email}")]
        public ActionResult<Customer> GetCustomersByName(string email)
        {
            var cust = _repo.FindByEmail(email);

            if (cust == null)
            {
                return NotFound();
            }

            return Ok(cust);
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomer(int id)
        {
            var cust = _repo.Find(id);

            if (cust == null)
            {
                return NotFound();
            }

            return Ok(cust);
        }

        // PUT: api/Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutCustomer(int id, Customer customer)
        {
            if (ModelState.IsValid)
            {
                if (id != customer.CustomerId)
                {
                    return BadRequest();
                }

                try
                {
                    _repo.Update(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_repo.Find(id) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
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

        // POST: api/Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Customer> PostCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(customer);
                return CreatedAtAction("GetCustomer", new { id = customer.CustomerId }, customer);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var cust = _repo.Find(id);
            if (cust == null)
            {
                return NotFound();
            }

            _repo.Remove(id);

            return NoContent();
        }
    }
}
