using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkopeiBackendAssignment.Entities.IRepository
{
    // Generic interface used to interact with database
    // Used to separate API Controller layer from data layer
    // Used with DI (Dependency Injection)
    // TEntity is replaced by model classes
    public interface IDataManager<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAll();
        Task<TEntity> Get(long id);
        Task<TEntity> Add(TEntity entity);
        Task<TEntity> Update(long id, TEntity entity);
        Task<TEntity> Delete(long id);
    }
}
