using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Core.Specifications;
using OrderManagement.Repository.Data;

namespace OrderManagement.Repository.Repositories
{
    public class GenericRepositories<T> : IGenericRepositories<T> where T : BaseEntity
    {
        private readonly OrderManagementDbContext _dbContext;

        public GenericRepositories(OrderManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        => await _dbContext.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
        => await _dbContext.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }
        public async Task<T> GetEntityWithSpecificationAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task<int> GetCountWithSpecification(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task AddAsync(T Item)
        =>await _dbContext.Set<T>().AddAsync(Item);

        public void Delete(T Item)
        => _dbContext.Set<T>().Remove(Item);

        public void Update(T Item)
        => _dbContext.Set<T>().Update(Item);
    }
}
