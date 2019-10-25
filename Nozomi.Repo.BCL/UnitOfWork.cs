using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nozomi.Data.Interfaces;

namespace Nozomi.Repo.BCL
{
    public class UnitOfWork<TContext>  : IUnitOfWork<TContext> where TContext : IDbContext
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

        public bool Commit(long userId = 0)
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

            return true;
        }

        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}