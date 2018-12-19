using System;
using System.Collections.Generic;
using System.Linq;

namespace Nozomi.Base.Core.Helpers.Enumerable
{
    public static class EnumerableHelper
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(
            this IEnumerable<T> enumerable,
            Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy<T, TKey>(keySelector).Select<IGrouping<TKey, T>, T>((Func<IGrouping<TKey, T>, T>) (grp => grp.First<T>()));
        }
    }
}