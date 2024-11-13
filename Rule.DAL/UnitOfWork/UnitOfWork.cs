using Rule.DAL.Context;
using Rule.DAL.Repositories;
using Rule.DAL.Repositories.Interfaces;
using Rule.DAL.UnitOfWork.Interfaces;

namespace Rule.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApiDbContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed = false;
        public UnitOfWork(ApiDbContext context)
        {
            _context = context;
            _repositories = [];
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (_repositories.ContainsKey(typeof(TEntity)))
            {
                return (IRepository<TEntity>)_repositories[typeof(TEntity)];
            }

            var repository = new Repository<TEntity>(_context);
            _repositories.Add(typeof(TEntity), repository);
            return repository;
        }

        public void SaveChanges() => _context.SaveChanges();
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    foreach (var repository in _repositories.Values)
                    {
                        (repository as IDisposable)?.Dispose();
                    }
                    _context.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task DisposeAsync()
        {
            Dispose();
            await Task.CompletedTask;
        }
    }
}
