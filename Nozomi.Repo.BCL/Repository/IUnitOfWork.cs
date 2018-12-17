using System;

namespace Nozomi.Repo.BCL.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        int SaveChanges(long userId = 0);
        int Commit(long userId = 0);
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork
    {
        TContext Context { get; }
    }
}