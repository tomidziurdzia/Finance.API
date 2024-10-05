using FinanceApp.Application.Specifications;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Infrastructure.Specifications;

public class SpecificationEvaluator<T> where T : class
{
    public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> specification)
    {
        if (specification.Criteria != null)
        {
            inputQuery = inputQuery.Where(specification.Criteria);
        }
        if (specification.OrderBy != null)
        {
            inputQuery = inputQuery.OrderBy(specification.OrderBy);
        }
        
        if (specification.OrderByDescending != null)
        {
            inputQuery = inputQuery.OrderBy(specification.OrderByDescending);
        }
        if (specification.IsPaginationEnable)
        {
            inputQuery = inputQuery.Skip(specification.Skip).Take(specification.Take);
        }
        inputQuery = specification.Includes!.Aggregate(inputQuery, (current, include) => current.Include(include)).AsSplitQuery().AsNoTracking();
        return inputQuery;
    }
}