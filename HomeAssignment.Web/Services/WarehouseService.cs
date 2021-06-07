using HomeAssignment.Web.Data;
using HomeAssignment.Web.Data.Repository;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeAssignment.Web.Services
{
    public interface IWarehouseService
    {
        IEnumerable<Product> ListAllProduct();
        IEnumerable<Inventory> ListAllInventory();
        Product GetProduct(int? id);
        Inventory GetInventory(int? id);
        void SellProduct(int id, int quantity);
        bool IsProductExist(int id);
        bool IsInventoryExist(int id);
        void RepopulateItems(string productJson, string inventoryJson);
    }

    public class WarehouseService : IWarehouseService
    {
        private readonly IRepository<Product> productRepository;
        private readonly IRepository<ProductInventory> productInventoryRepository;
        private readonly IRepository<Inventory> inventoryRepository;

        public WarehouseService(IRepository<Product> productRepository, IRepository<ProductInventory> productInventoryRepository, IRepository<Inventory> inventoryRepository)
        {
            this.productRepository = productRepository;
            this.productInventoryRepository = productInventoryRepository;
            this.inventoryRepository = inventoryRepository;
        }

        public Inventory GetInventory(int? id)
        {
            return inventoryRepository.Query.Include(x => x.ProductInventory).FirstOrDefault(m => m.Id == id);
        }

        public Product GetProduct(int? id)
        {
            return productRepository.Query.Include(x => x.ProductInventory).ThenInclude(y => y.Inventory).FirstOrDefault(m => m.Id == id);
        }

        public bool IsInventoryExist(int id)
        {
            return inventoryRepository.Query.FirstOrDefault(x => x.Id == id) != null;
        }

        public bool IsProductExist(int id)
        {
            return productRepository.Query.FirstOrDefault(x => x.Id == id) != null;
        }

        public IEnumerable<Inventory> ListAllInventory()
        {
            return inventoryRepository.GetAll();
        }

        public IEnumerable<Product> ListAllProduct()
        {
            return productRepository.GetAll();
        }

        public void RepopulateItems(string productsJson, string inventoryJson)
        {
            inventoryJson = JObject.Parse(inventoryJson.ToString())["inventory"].ToString();
            IList<Inventory> inventories = JsonConvert.DeserializeObject<List<Inventory>>(inventoryJson);
            IList<Product> products = new List<Product>();
            IList<ProductInventory> productInventories = new List<ProductInventory>();


            int productId = 1;
            int articleId = 1;
            var productJTokens = JObject.Parse(productsJson.ToString())["products"].Children().ToList();
            foreach (var productJToken in productJTokens)
            {
                Product product = productJToken.ToObject<Product>();
                product.Id = productId;
                products.Add(product);

                var articleJTokens = productJToken["contain_articles"].Children().ToList();
                foreach (var articleJToken in articleJTokens)
                {
                    ProductInventory productInventory = articleJToken.ToObject<ProductInventory>();
                    productInventory.Id = articleId++;
                    productInventory.ProductId = productId;
                    productInventories.Add(productInventory);
                }

                productId++;
            }

            // Calculate quantity
            for (int i = 0; i < products.Count; i++)
            {
                var product = products[i];
                int lowestDivisionResult = Int32.MaxValue;

                foreach (var productInventory in productInventories)
                {
                    if (productInventory.ProductId != product.Id)
                        continue;

                    int inventoryStock = inventories.Single(x => x.Id == productInventory.InventoryId).Stock;
                    int divisionResult = inventoryStock / productInventory.Amount;
                    lowestDivisionResult = divisionResult < lowestDivisionResult ? divisionResult : lowestDivisionResult;
                }
                products[i].Quantity = lowestDivisionResult;
            }

            // Delete all data
            productInventoryRepository.DeleteAll();
            productRepository.DeleteAll();
            inventoryRepository.DeleteAll();

            // Repopulate data
            inventoryRepository.AddRange(inventories);
            productRepository.AddRange(products);
            productInventoryRepository.AddRange(productInventories);

            productInventoryRepository.Save();
            productRepository.Save();
            inventoryRepository.Save();

        }

        public void SellProduct(int id, int quantity)
        {
            var product = GetProduct(id);
            var sellQuantity = quantity;

            if (sellQuantity > product.Quantity)
                throw new ArgumentException("Sell quantity is higher than available product stock.");

            // Update inventory
            foreach (var productInventory in product.ProductInventory)
            {
                productInventory.Inventory.Stock = productInventory.Inventory.Stock - (productInventory.Amount * sellQuantity);
            }

            productRepository.Save();

            // Updated related product quantity using the same inventory
            foreach (var productInventory in product.ProductInventory)
            {
                var relatedProducts = productInventoryRepository.Query.Include(x => x.Product).ThenInclude(x => x.ProductInventory).ThenInclude(x => x.Inventory).Include(y => y.Inventory).Where(x => x.InventoryId == productInventory.InventoryId).Select(x => x.Product);
                foreach (var relatedProduct in relatedProducts)
                {
                    var relatedProductProductInventoryList = relatedProduct.ProductInventory;
                    int lowestDivisionResult = Int32.MaxValue;
                    foreach (var relatedProductProductInventory in relatedProductProductInventoryList)
                    {
                        int divisionResult = relatedProductProductInventory.Inventory.Stock / relatedProductProductInventory.Amount;
                        lowestDivisionResult = divisionResult < lowestDivisionResult ? divisionResult : lowestDivisionResult;
                    }
                    relatedProduct.Quantity = lowestDivisionResult;
                }
            }

            productInventoryRepository.Save();
        }
    }
}
