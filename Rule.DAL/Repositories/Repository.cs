using Microsoft.EntityFrameworkCore;
using Rule.DAL.Context;
using Rule.DAL.Repositories.Interfaces;
using Rule.DAL.Specifications.Interfaces;
using System.Linq.Expressions;

namespace Rule.DAL.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApiDbContext _context;

        public Repository(ApiDbContext context) => _context = context;

        public IQueryable<TEntity> Get() => _context.Set<TEntity>();

        public async Task AddAsync(TEntity entity) => await _context.Set<TEntity>().AddAsync(entity);

        public async Task DeleteAsync(TEntity entity) => await Task.Run(() => _context.Set<TEntity>().Remove(entity));

        public ICollection<TEntity> Find(Expression<Func<TEntity, bool>> predicate) =>
            _context.Set<TEntity>().Where(predicate).ToList();

        public async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate) =>
            await _context.Set<TEntity>().Where(predicate).ToListAsync();

        public async Task<ICollection<TEntity>> GetAllAsync(ISpecification<TEntity>? spec = null)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            if (spec != null)
            {
                query = ApplySpecification(query, spec);
            }

            return await query.ToListAsync();
        }

        public TEntity? GetById(object id) => _context.Set<TEntity>().Find(id);

        public async Task<TEntity?> GetByIdAsync(object id) => await _context.Set<TEntity>().FindAsync(id);

        public async Task UpdateAsync(TEntity entity) => await Task.Run(() => _context.Set<TEntity>().Update(entity));

        private IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> query, ISpecification<TEntity> spec)
        {
            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);

            if (spec.OrderByDescending != null)
                query = query.OrderByDescending(spec.OrderByDescending);

            return query;
        }
    }
}
