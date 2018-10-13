using Microsoft.EntityFrameworkCore;
using Nozomi.Data.CurrencyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Counter.SDK.SharedModels;
using Nozomi.Data.WebModels;
using Nozomi.Data.WebModels.LoggingModels;
using Nozomi.Repo.Data.Mappings.CurrencyModels;
using Nozomi.Repo.Data.Mappings.WebModels;
using Nozomi.Repo.Data.Mappings.WebModels.LoggingModels;

namespace Nozomi.Repo.Data
{
    public class NozomiDbContext : DbContext, IDbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyPair> CurrencyPairs { get; set; }
        public DbSet<CurrencyPairRequest> CurrencyPairRequests { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<PartialCurrencyPair> PartialCurrencyPairs { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestComponent> RequestComponents { get; set; }
        public DbSet<RequestComponentDatum> RequestComponentData { get; set; }
        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<RequestProperty> RequestProperties { get; set; }
        public DbSet<Source> Sources { get; set; }
        
        // Introducing Compiled Queries
        private static readonly Func<NozomiDbContext, RequestType, IEnumerable<CurrencyPairRequest>> _getCurrencyPairRequestByRequestType =
            EF.CompileQuery((NozomiDbContext context, RequestType type) =>
                context.CurrencyPairRequests
                    .AsQueryable()
                    .Include(cpr => cpr.RequestComponents)
                        .ThenInclude(rc => rc.RequestComponentData)
                    .Include(r => r.CurrencyPair)
                    .Include(r => r.RequestProperties)
                    .Where(r => r.IsEnabled && r.DeletedAt == null
                                            && r.RequestType == type
                                            && r.RequestComponents.Any(rc => !rc.RequestComponentData.Any() 
                                            || (DateTime.UtcNow > rc.RequestComponentData
                                                    .OrderByDescending(rcd => rcd.CreatedAt)
                                                    .FirstOrDefault().CreatedAt.AddMilliseconds(r.Delay)))));

        public IEnumerable<CurrencyPairRequest> GetCurrencyPairRequestByRequestType(RequestType requestType)
        {
            return _getCurrencyPairRequestByRequestType(this, requestType);
        }

        public NozomiDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var currencyMap = new CurrencyMap(modelBuilder.Entity<Currency>());
            var currencyPairMap = new CurrencyPairMap(modelBuilder.Entity<CurrencyPair>());
            var currencyPairRequestMap = new CurrencyPairRequestMap(modelBuilder.Entity<CurrencyPairRequest>());
            var currencyTypeMap = new CurrencyTypeMap(modelBuilder.Entity<CurrencyType>());
            var partialCurrencyPairMap = new PartialCurrencyPairMap(modelBuilder.Entity<PartialCurrencyPair>());
            var requestMap = new RequestMap(modelBuilder.Entity<Request>());
            var requestComponentMap = new RequestComponentMap(modelBuilder.Entity<RequestComponent>());
            var requestComponentDatumMap = new RequestComponentDatumMap(modelBuilder.Entity<RequestComponentDatum>());
            var requestLogMap = new RequestLogMap(modelBuilder.Entity<RequestLog>());
            var requestPropertyMap = new RequestPropertyMap(modelBuilder.Entity<RequestProperty>());
            var sourceMap = new SourceMap(modelBuilder.Entity<Source>());
            
            base.OnModelCreating(modelBuilder);
        }
        
        public int SaveChanges(long userId = 0)
        {
            AddTimestamps(userId);
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps(0);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> SaveChangesAsync(long userId = 0,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            AddTimestamps(userId);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void AddTimestamps(long userId = 0)
        {
            var entities = ChangeTracker.Entries().Where(x =>
                x.Entity is BaseEntityModel && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                switch (entity.State)
                {
                    case EntityState.Added:
                        ((BaseEntityModel) entity.Entity).CreatedAt = DateTime.UtcNow;
                        ((BaseEntityModel) entity.Entity).CreatedBy = userId;
                        break;
                    case EntityState.Deleted:
                        ((BaseEntityModel) entity.Entity).DeletedAt = DateTime.UtcNow;
                        ((BaseEntityModel) entity.Entity).DeletedBy = userId;
                        break;
                }

                ((BaseEntityModel) entity.Entity).ModifiedAt = DateTime.UtcNow;
                ((BaseEntityModel) entity.Entity).ModifiedBy = userId;
            }
        }
    }
}
