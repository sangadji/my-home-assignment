using System.Collections.Generic;
using System.Linq;

namespace HomeAssignment.Web.Data.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> GetAll();
        IQueryable<TEntity> Query { get; }
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteAll();
        void Save();
    }
}
