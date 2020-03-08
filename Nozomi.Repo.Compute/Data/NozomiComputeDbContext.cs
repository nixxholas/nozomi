using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nozomi.Base.BCL;
using Nozomi.Data.Models.Web;
using Nozomi.Repo.Compute.Data.Mappings;

namespace Nozomi.Repo.Compute.Data
{
    public class NozomiComputeDbContext : DbContext
    {
        public DbSet<Nozomi.Data.Models.Web.Compute> Computes { get; set; }
        public DbSet<ComputeExpression> ComputeExpressions { get; set; }
        public DbSet<ComputeValue> ComputeValues { get; set; }
        public DbSet<SubCompute> SubComputes { get; set; }
        
        public NozomiComputeDbContext(DbContextOptions<NozomiComputeDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.UseIdentityColumns();

            var computeMap = new ComputeMap(modelBuilder.Entity<Nozomi.Data.Models.Web.Compute>());
            modelBuilder.Entity<Nozomi.Data.Models.Web.Compute>().UseXminAsConcurrencyToken();

            var computeExpressionMap = new ComputeExpressionMap(modelBuilder.Entity<ComputeExpression>());
            modelBuilder.Entity<ComputeExpression>().UseXminAsConcurrencyToken();

            var computeValueMap = new ComputeValueMap(modelBuilder.Entity<ComputeValue>());
            modelBuilder.Entity<ComputeValue>().UseXminAsConcurrencyToken();

            var subComputeMap = new SubComputeMap(modelBuilder.Entity<SubCompute>());
            modelBuilder.Entity<SubCompute>().UseXminAsConcurrencyToken();
            
            // https://stackoverflow.com/questions/37578359/how-do-i-configure-entity-framework-to-allow-database-generate-uuid-for-postgres
            modelBuilder.HasPostgresExtension("uuid-ossp");
        }

        public int SaveChanges(string userId)
        {
            AddTimestamps(userId);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps(string.Empty);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(string userId,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps(userId);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void AddTimestamps(string userId)
        {
            try
            {
                var entities = ChangeTracker.Entries().Where(x =>
                    x.Entity is Entity && (x.State == EntityState.Added || x.State == EntityState.Modified));

                foreach (var entity in entities)
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            ((Entity) entity.Entity).CreatedAt = DateTime.UtcNow;
                            if (!string.IsNullOrWhiteSpace(userId))
                                ((Entity) entity.Entity).CreatedById = userId;
                            break;
                        case EntityState.Deleted:
                            ((Entity) entity.Entity).DeletedAt = DateTime.UtcNow;
                            if (!string.IsNullOrWhiteSpace(userId))
                                ((Entity) entity.Entity).DeletedById = userId;
                            break;
                    }

                    ((Entity) entity.Entity).ModifiedAt = DateTime.UtcNow;
                    if (!string.IsNullOrWhiteSpace(userId))
                        ((Entity) entity.Entity).ModifiedById = userId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[NozomiDbContext]: " + ex);
            }
        }
    }
}