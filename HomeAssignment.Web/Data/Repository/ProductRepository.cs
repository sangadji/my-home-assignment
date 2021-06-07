using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeAssignment.Web.Data.Repository
{
    public class ProductRepository : IRepository<Product>
    {
        public IQueryable<Product> Query => dbContext.Product.AsQueryable();
        private HomeAssignmentDbContext dbContext;

        public ProductRepository(HomeAssignmentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(Product entity)
        {
            dbContext.Add(entity);
        }

        public void AddRange(IEnumerable<Product> entities)
        {
            dbContext.AddRange(entities);
        }

        public void Delete(Product entity)
        {
            dbContext.Remove(entity);
        }

        public List<Product> GetAll()
        {
            return dbContext.Product.Include(x => x.ProductInventory).ThenInclude(y => y.Inventory).ToList();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void DeleteAll()
        {
            dbContext.Product.RemoveRange(dbContext.Product);
        }
    }
}
