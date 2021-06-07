using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeAssignment.Web.Data
{
    public class HomeAssignmentDbContext : DbContext
    {
        public HomeAssignmentDbContext(DbContextOptions<HomeAssignmentDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<ProductInventory> ProductInventory { get; set; }
    }

    public static class DbInitializer
    {
        public static void Initialize(HomeAssignmentDbContext context)
        {
            context.Database.EnsureCreated();

            //Look for any Product.
            if (context.Product.Any())
            {
                return;   // DB has been seeded
            }

            var products = new Product[]
            {
                new Product {Id = 1, Name = "Dining Chair"},
                new Product {Id = 2, Name = "Dining Table"}
            };

            foreach (Product p in products)
            {
                context.Product.Add(p);
            }
            context.SaveChanges();

            var inventories = new Inventory[]
            {
                new Inventory {Id = 1, Name = "leg", Stock = 12},
                new Inventory {Id = 2, Name = "screw", Stock = 17},
                new Inventory {Id = 3, Name = "seat", Stock = 2},
                new Inventory {Id = 4, Name = "table top", Stock = 1}
            };

            foreach (Inventory i in inventories)
            {
                context.Inventory.Add(i);
            }
            context.SaveChanges();

            var productInventories = new ProductInventory[]
            {
                new ProductInventory {Id = 1, ProductId = 1, InventoryId = 1, Amount = 4},
                new ProductInventory {Id = 2, ProductId = 1, InventoryId = 2, Amount = 8},
                new ProductInventory {Id = 3, ProductId = 1, InventoryId = 3, Amount = 1},
                new ProductInventory {Id = 4, ProductId = 2, InventoryId = 1, Amount = 4},
                new ProductInventory {Id = 5, ProductId = 2, InventoryId = 2, Amount = 8},
                new ProductInventory {Id = 6, ProductId = 2, InventoryId = 4, Amount = 1}
            };

            foreach (ProductInventory pi in productInventories)
            {
                context.ProductInventory.Add(pi);
            }
            context.SaveChanges();
        }
    }
}

