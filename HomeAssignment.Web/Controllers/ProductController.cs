using Microsoft.AspNetCore.Mvc;
using HomeAssignment.Web.Models;
using HomeAssignment.Web.Services;

namespace HomeAssignment.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWarehouseService warehouseService;

        public ProductController(IWarehouseService warehouseService)
        {
            this.warehouseService = warehouseService;
        }

        // GET: Product
        public IActionResult Index()
        {
            return View(warehouseService.ListAllProduct());
        }

        // GET: Product/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = warehouseService.GetProduct(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Edit/5
        public IActionResult Sell(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = warehouseService.GetProduct(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(new SellProductViewModel { Product = product });
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Sell(int id, SellProductViewModel viewModel)
        {
            if (id != viewModel.Product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                warehouseService.SellProduct(id, viewModel.Quantity);
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        private bool ProductExists(int id)
        {
            return warehouseService.IsProductExist(id);
        }
    }
}
