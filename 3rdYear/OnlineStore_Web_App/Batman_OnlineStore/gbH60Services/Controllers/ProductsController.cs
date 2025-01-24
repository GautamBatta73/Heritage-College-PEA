using gbH60Services.DAL;
using gbH60Services.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gbH60Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Products
        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return _repo.GetAll();
        }

        [HttpGet("name/{name}")]
        public ActionResult<IEnumerable<Product>> GetProductsByName(string name)
        {
            var prods = _repo.FindByName(name);

            if (prods == null || !prods.Any())
            {
                return NotFound();
            }

            return Ok(prods);
        }

        [HttpGet("GetWithCategory")]
        public ActionResult<IEnumerable<JoinProductCategory>> GetProductsWithCategory()
        {
            var prods = _repo.GetByCategory();

            if (prods == null || !prods.Any())
            {
                return NotFound();
            }

            return Ok(prods);
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public ActionResult<Product> GetProduct(int id)
        {
            var product = _repo.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public IActionResult PutProduct(int id, Product product)
        {
            if (ModelState.IsValid)
            {
                if (id != product.ProductId)
                {
                    return BadRequest();
                }

                try
                {
                    _repo.Update(product);
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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<Product> PostProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                _repo.Add(product);
                return CreatedAtAction("GetProduct", new { id = product.ProductId }, product);
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _repo.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _repo.Remove(id);

            return NoContent();
        }
    }
}
