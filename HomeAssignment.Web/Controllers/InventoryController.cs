using Microsoft.AspNetCore.Mvc;
using HomeAssignment.Web.Services;

namespace HomeAssignment.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IWarehouseService warehouseService;

        public InventoryController(IWarehouseService warehouseService)
        {
            this.warehouseService = warehouseService;
        }

        // GET: Inventory
        public IActionResult Index()
        {
            return View(warehouseService.ListAllInventory());
        }

        // GET: Inventory/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = warehouseService.GetInventory(id);
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        private bool InventoryExists(int id)
        {
            return warehouseService.IsInventoryExist(id);
        }
    }
}
