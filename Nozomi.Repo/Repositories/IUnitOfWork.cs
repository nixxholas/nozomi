using System;
using Nozomi.Repo.Data;

namespace Nozomi.Repo.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        int SaveChanges(long userId = 0);
        int Commit(long userId = 0);
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : IDbContext
    {
        TContext Context { get; }
    }
}