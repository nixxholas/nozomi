using System;

namespace Nozomi.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        bool Commit(long userId = 0);
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : IDbContext
    {
        TContext Context { get; }
    }
}