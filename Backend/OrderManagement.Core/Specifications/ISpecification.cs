using OrderManagement.Core.Entities;
using System.Linq.Expressions;

namespace OrderManagement.Core.Specifications
{
    public interface ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescening { get; set; }
        public int Skip { get; set; }
        public bool IsPaginated { get; set; }
        public int Take { get; set; }
    }
}
