using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeAssignment.Web.Data.Repository
{
    public class ProductInventoryRepository : IRepository<ProductInventory>
    {
        public IQueryable<ProductInventory> Query => dbContext.ProductInventory.AsQueryable();
        private HomeAssignmentDbContext dbContext;

        public ProductInventoryRepository(HomeAssignmentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(ProductInventory entity)
        {
            dbContext.Add(entity);
        }

        public void AddRange(IEnumerable<ProductInventory> entities)
        {
            dbContext.AddRange(entities);
        }

        public void Delete(ProductInventory entity)
        {
            dbContext.Remove(entity);
        }

        public List<ProductInventory> GetAll()
        {
            return dbContext.ProductInventory.Include(x => x.Product).Include(y => y.Inventory).ToList();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void DeleteAll()
        {
            dbContext.ProductInventory.RemoveRange(dbContext.ProductInventory);
        }
    }
}
