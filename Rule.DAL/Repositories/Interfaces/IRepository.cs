using System.Linq.Expressions;
using Rule.DAL.Specifications.Interfaces;

namespace Rule.DAL.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> Get();
        Task<ICollection<TEntity>> GetAllAsync(ISpecification<TEntity>? spec = null);
        Task<TEntity?> GetByIdAsync(object id);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
