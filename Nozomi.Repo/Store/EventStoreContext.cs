using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Nozomi.Base.Core.Events;
using Nozomi.Data.Interfaces;
using Nozomi.Repo.Store.Mappings;

namespace Nozomi.Repo.Store
{
    public class EventStoreContext : DbContext, IDbContext
    {
        private readonly IHostingEnvironment _env;

        public DbSet<StoredEvent> StoredEvent { get; set; }

        public EventStoreContext(IHostingEnvironment env)
        {
            _env = env;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new StoredEventMap());

            base.OnModelCreating(modelBuilder);
        }

        public void Dispose()
        {
            base.Dispose();
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public int SaveChanges(long userId = 0)
        {
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(long userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void AddTimestamps(long userId = 0)
        {
            throw new System.NotImplementedException();
        }
    }
}