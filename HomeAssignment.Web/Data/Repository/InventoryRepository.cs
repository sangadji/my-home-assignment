using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeAssignment.Web.Data.Repository
{
    public class InventoryRepository : IRepository<Inventory>
    {
        public IQueryable<Inventory> Query => dbContext.Inventory.AsQueryable();
        private HomeAssignmentDbContext dbContext;

        public InventoryRepository(HomeAssignmentDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Add(Inventory entity)
        {
            dbContext.Add(entity);
        }

        public void AddRange(IEnumerable<Inventory> entities)
        {
            dbContext.AddRange(entities);
        }

        public void Delete(Inventory entity)
        {
            dbContext.Remove(entity);
        }

        public List<Inventory> GetAll()
        {
            return dbContext.Inventory.Include(x => x.ProductInventory).ToList();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void DeleteAll()
        {
            dbContext.Inventory.RemoveRange(dbContext.Inventory);
        }
    }
}
