using System;
using System.Collections.Generic;
using Nozomi.Repo.BCL.Context;

namespace Nozomi.Repo.BCL.Repository
{
    public class UnitOfWork<TContext>  : IRepositoryFactory, IUnitOfWork<TContext>, IUnitOfWork
        where TContext : IDbContext
    {
        private Dictionary<Type, object> _repositories;

        public UnitOfWork(TContext context)
        {
            Context = context;
        }

        public TContext Context { get; }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class 
        {
            if (_repositories == null)
            {
                _repositories = new Dictionary<Type, object>();
            }

            var type = typeof(TEntity);
            if (!_repositories.ContainsKey(type))
            {
                _repositories[type]= new Repository<TEntity>(Context);
            }
            return (IRepository<TEntity>) _repositories[type];
        }

        public int SaveChanges(long userId = 0)
        {
            return Context.SaveChanges();
        }

        public int Commit(long userId = 0)
        {
            return Context.SaveChanges(userId);
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}