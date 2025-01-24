using gbH60A02.DAL;
using gbH60A02.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace gbH60A02.Controllers
{
    public class ProductCategoriesController : Controller
    {
        private readonly IProductCategoryRepository _repo;

        public ProductCategoriesController(IProductCategoryRepository repo)
        {
            _repo = repo;
        }

        [Authorize(Roles = "Manager,Clerk")]
        public async Task<IActionResult> Index()
        {
            return View(await _repo.DisplayAll());
        }

        [Authorize(Roles = "Manager,Clerk")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Manager,Clerk")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync([Bind("CategoryId,ProdCat,ImageURL")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.Add(productCategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(productCategory);
                }

            }
            return View(productCategory);
        }

        [Authorize(Roles = "Manager,Clerk")]
        public async Task<IActionResult> DetailsAsync(int? id)
        {
            var productCategory = await _repo.Find(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            ViewData["Products"] = await _repo.DisplayProductsOfCategory(id);
            return View(productCategory);
        }

        [Authorize(Roles = "Manager,Clerk")]
        public async Task<IActionResult> EditAsync(int? id)
        {
            var productCategory = await _repo.Find(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        [Authorize(Roles = "Manager,Clerk")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(int id, [Bind("CategoryId,ProdCat,ImageURL")] ProductCategory productCategory)
        {
            if (id != productCategory.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.Update(productCategory);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    return View(productCategory);
                }
            }

            return View(productCategory);
        }

        [Authorize(Roles = "Manager,Clerk")]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            ViewData["FKError"] = false;

            var productCategory = await _repo.Find(id);

            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        [Authorize(Roles = "Manager,Clerk")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            var productCategory = await _repo.Find(id);
            try
            {
                await _repo.Remove(id);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewData["FKError"] = true;

                ViewData["Products"] = await _repo.DisplayProductsOfCategory(id);
            }
            catch (Exception)
            {
                return View(productCategory);
            }

            return View(productCategory);
        }
    }
}
