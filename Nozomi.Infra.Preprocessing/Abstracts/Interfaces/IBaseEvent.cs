using System;

namespace Nozomi.Preprocessing.Abstracts.Interfaces
{
    public interface IBaseEvent<out T> where T : class
    {
        long QueryCount(Func<T, bool> predicate);
    }
}