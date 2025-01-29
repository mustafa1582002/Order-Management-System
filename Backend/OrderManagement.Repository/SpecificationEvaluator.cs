using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Specifications;

namespace OrderManagement.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        // the difference between Iqueryable and Ienumerable is Iquerable load data in db server while ienum load data in
        // server which is a big load for server
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> Spec)
        {
            var Query = inputQuery;
            if (Spec.Criteria is not null)
            {
                Query = Query.Where(Spec.Criteria);
            }

            if (Spec.OrderBy is not null)
            {
                Query = Query.OrderBy(Spec.OrderBy);
            }
            if (Spec.OrderByDescening is not null)
            {
                Query = Query.OrderByDescending(Spec.OrderByDescening);
            }
            if (Spec.IsPaginated == true)
                Query.Skip(Spec.Skip).Take(Spec.Take);

            Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeQuery) => CurrentQuery.Include(IncludeQuery));
            return Query;
        }
    }
}
