using OrderManagement.Core.Entities;
using OrderManagement.Core.Specifications;

namespace OrderManagement.Core.Interfaces
{
    public interface IGenericRepositories<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        //with Specs
        Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> spec);
        Task<T> GetEntityWithSpecificationAsync(ISpecification<T> spec);
        Task<int> GetCountWithSpecification(ISpecification<T> spec);
        Task AddAsync(T Item);
        void Delete(T Item);
        void Update(T Item);
    }
}
