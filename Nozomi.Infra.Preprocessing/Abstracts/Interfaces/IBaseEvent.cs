using System;
using System.Linq.Expressions;

namespace Nozomi.Preprocessing.Abstracts.Interfaces
{
    public interface IBaseEvent<T> where T : class
    {
        long QueryCount(Expression<Func<T, bool>> predicate);
    }
}