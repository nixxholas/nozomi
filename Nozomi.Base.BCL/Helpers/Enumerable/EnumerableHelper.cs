using System;
using System.Collections.Generic;
using System.Linq;
using Nozomi.Base.BCL.Helpers.Attributes;

namespace Nozomi.Base.BCL.Helpers.Enumerable
{
    public static class EnumerableHelper
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(
            this IEnumerable<T> enumerable,
            Func<T, TKey> keySelector)
        {
            return enumerable.GroupBy(keySelector).Select(grp => grp.First());
        }
        
        public static bool IsComparable(Enum value)
        {
            var output = false;
            var type = value.GetType();
            var fi = type.GetField(value.ToString());
            if (fi.GetCustomAttributes(typeof(Comparable),false) is Comparable[] attrs)
            {
                output = attrs[0].Value;
            }
            return output;
        }
    }
}