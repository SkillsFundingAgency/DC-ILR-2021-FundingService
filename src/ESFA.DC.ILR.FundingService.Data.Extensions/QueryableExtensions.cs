using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ESFA.DC.ILR.FundingService.Data.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TResult> CaseInsensitiveJoin<TOuter, TInner, TKey, TResult>(this IQueryable<TOuter> outer, IEnumerable<TInner> inner, Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            return outer.Join(
                    inner,
                    outerKeySelector,
                    innerKeySelector,
                    resultSelector,
                    StringComparer.OrdinalIgnoreCase as IEqualityComparer<TKey>);
        }
    }
}