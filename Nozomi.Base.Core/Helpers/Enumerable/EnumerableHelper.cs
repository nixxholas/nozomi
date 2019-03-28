using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Nozomi.Base.Core.Helpers.Attributes;

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
        
        public static bool IsComparable(Enum value)
        {
            var output = false;
            var type = value.GetType();
            var fi = type.GetField(value.ToString());
            var attrs = fi.GetCustomAttributes(typeof(Comparable),false) as Comparable[];
            if (attrs != null)
            {
                output = attrs[0].Value;
            }
            return output;
        }
    }
}