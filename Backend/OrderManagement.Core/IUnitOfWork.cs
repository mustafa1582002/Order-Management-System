using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Core
{
    public interface IUnitOfWork:IAsyncDisposable
    {
        IGenericRepositories<TEntity> Repository<TEntity>() where TEntity:BaseEntity;
        Task<int> CompleteAsync();
    }
}
