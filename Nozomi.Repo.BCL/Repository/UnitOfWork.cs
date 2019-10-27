using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nozomi.Repo.BCL.Context;

namespace Nozomi.Repo.BCL.Repository
{
    public class UnitOfWork<TContext>  : IRepositoryFactory, IUnitOfWork<TContext>
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

        public int Commit(string userId = null)
        {
            bool saveFailed;
            do
            {
                saveFailed = false;
                try
                {
                    Context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update original values from the database
                    var entry = ex.Entries.Single();
                    entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                }

            } while (saveFailed);

            return 1;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}