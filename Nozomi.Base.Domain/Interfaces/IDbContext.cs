using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nozomi.Data.Interfaces
{
    public interface IDbContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        
        int SaveChanges(long userId = 0);

        Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default(CancellationToken));

        Task<int> SaveChangesAsync(long userId,
            CancellationToken cancellationToken = default(CancellationToken));

        void AddTimestamps(long userId = 0);
    }
}