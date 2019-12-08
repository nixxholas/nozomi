using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nozomi.Repo.BCL.Context
{
    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        
        int SaveChanges(string userId = null);

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> SaveChangesAsync(string userId,
            CancellationToken cancellationToken = default(CancellationToken));

        void AddTimestamps(string userId);
    }
}