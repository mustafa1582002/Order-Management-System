using OrderManagement.Core;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Repository.Data;
using OrderManagement.Repository.Repositories;
using System.Collections;

namespace OrderManagement.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OrderManagementDbContext _dbContext;
        private Hashtable _repositories;

        public UnitOfWork(OrderManagementDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
            => await _dbContext.SaveChangesAsync();

        public async ValueTask DisposeAsync()
            =>await _dbContext.DisposeAsync();

        public IGenericRepositories<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var Repository = new GenericRepositories<TEntity>(_dbContext);
                _repositories.Add(type, Repository);
            }
            return _repositories[type] as IGenericRepositories<TEntity>;
        }
    }
}
