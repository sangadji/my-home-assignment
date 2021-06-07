using HomeAssignment.Web.Models;
using HomeAssignment.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HomeAssignment.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWarehouseService warehouseService;

        public HomeController(IWarehouseService warehouseService)
        {
            this.warehouseService = warehouseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("save")]
        [HttpPost]
        public IActionResult Upload(IFormFile inventoryFile, IFormFile productFile)
        {
            if (inventoryFile == null || inventoryFile.Length == 0)
            {
                return Content("Inventory.json cannot be empty");
            }

            if (productFile == null || productFile.Length == 0)
            {
                return Content("Product.json cannot be empty");
            }

            var inventoryJson = new StringBuilder();
            using (var reader = new StreamReader(inventoryFile.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    inventoryJson.AppendLine(reader.ReadLine());
            }
            var productJson = new StringBuilder();
            using (var reader = new StreamReader(productFile.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    productJson.AppendLine(reader.ReadLine());
            }

            warehouseService.RepopulateItems(productJson.ToString(), inventoryJson.ToString());

            ViewBag.product = productFile;
            return RedirectToAction("Index", "Product");
        }
    }
}
