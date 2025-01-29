using OrderManagement.Core.Entities;
using System.Linq.Expressions;

namespace OrderManagement.Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescening { get; set; }
        public int Skip { get; set; }
        public bool IsPaginated { get; set; }
        public int Take { get; set; }
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public void AddOrderBy(Expression<Func<T, object>> orderByExp)
        {
            OrderBy = orderByExp;
        }
        public void AddOrderByDescening(Expression<Func<T, object>> orderByDescExp)
        {
            OrderByDescening = orderByDescExp;
        }
        public void ApplyPagination(int skip, int take)
        {
            IsPaginated = true;
            Skip = skip;
            Take = take;
        }
    }
}
