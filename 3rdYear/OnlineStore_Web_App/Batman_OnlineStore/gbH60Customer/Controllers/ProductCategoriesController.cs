using gbH60Customer.DAL;
using Microsoft.AspNetCore.Mvc;

namespace gbH60Customer.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly IProductCategoryRepository _repo;

        public ProductCategoriesController(IProductCategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _repo.DisplayAll());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string name)
        {
            ViewData["name"] = name ?? "";
            if (string.IsNullOrEmpty(name))
            {
                return View(await _repo.DisplayAll());
            }
            else
            {
                return View(await _repo.FindByName(name));
            }
        }

        public async Task<IActionResult> DetailsAsync(int? id)
        {
            var productCategory = await _repo.Find(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            ViewData["Products"] = await _repo.DisplayProductsOfCategory(id);
            ViewData["CustId"] = (HttpContext.Session.GetInt32("CustId") ?? 0);
            return View(productCategory);
        }
    }
}
