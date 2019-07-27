using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Base.Core;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.Data.Mappings.CurrencyModels;
using Nozomi.Repo.Data.Mappings.WebModels;
using Nozomi.Repo.Data.Mappings.WebModels.AnalyticalModels;
using Nozomi.Repo.Data.Mappings.WebModels.WebsocketModels;

namespace Nozomi.Repo.Data
{
    public class NozomiDbContext : DbContext, IDbContext
    {
        private readonly ILogger<NozomiDbContext> _logger;
        
        public DbSet<AnalysedComponent> AnalysedComponents { get; set; }
        public DbSet<AnalysedHistoricItem> AnalysedHistoricItems { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyPair> CurrencyPairs { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<CurrencySource> CurrencySources { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestComponent> RequestComponents { get; set; }
        public DbSet<RcdHistoricItem> RcdHistoricItems { get; set; }
        public DbSet<RequestProperty> RequestProperties { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<WebsocketCommand> WebsocketCommands { get; set; }
        public DbSet<WebsocketCommandProperty> WebsocketCommandProperties { get; set; }
        
        public NozomiDbContext(DbContextOptions<NozomiDbContext> options,
            ILogger<NozomiDbContext> logger) : base(options)
        {
            _logger = logger;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ForNpgsqlUseIdentityColumns();
            
            var analysedComponentMap = new AnalysedComponentMap(modelBuilder.Entity<AnalysedComponent>());
            modelBuilder.Entity<AnalysedComponent>().ForNpgsqlUseXminAsConcurrencyToken();

            var analysedHistoricItemMap = new AnalysedHistoricItemMap(modelBuilder.Entity<AnalysedHistoricItem>());
            modelBuilder.Entity<AnalysedHistoricItem>().ForNpgsqlUseXminAsConcurrencyToken();

            var currencyMap = new CurrencyMap(modelBuilder.Entity<Currency>());
            modelBuilder.Entity<Currency>().ForNpgsqlUseXminAsConcurrencyToken();

            var currencyPairMap = new CurrencyPairMap(modelBuilder.Entity<CurrencyPair>());
            modelBuilder.Entity<CurrencyPair>().ForNpgsqlUseXminAsConcurrencyToken();

            var currencyTypeMap = new CurrencyTypeMap(modelBuilder.Entity<CurrencyType>());
            modelBuilder.Entity<CurrencyType>().ForNpgsqlUseXminAsConcurrencyToken();

            var requestMap = new RequestMap(modelBuilder.Entity<Request>());
            modelBuilder.Entity<Request>().ForNpgsqlUseXminAsConcurrencyToken();

            var webSocketCommandMap = new WebsocketCommandMap(modelBuilder.Entity<WebsocketCommand>());
            modelBuilder.Entity<WebsocketCommand>().ForNpgsqlUseXminAsConcurrencyToken();

            var webSocketCommandPropertyMap = new WebsocketCommandPropertyMap(modelBuilder.Entity<WebsocketCommandProperty>());
            modelBuilder.Entity<WebsocketCommandProperty>().ForNpgsqlUseXminAsConcurrencyToken();

            var requestComponentMap = new RequestComponentMap(modelBuilder.Entity<RequestComponent>());
            modelBuilder.Entity<RequestComponent>().ForNpgsqlUseXminAsConcurrencyToken();

            var rcdHistoricItemMap = new RcdHistoricItemMap(modelBuilder.Entity<RcdHistoricItem>());
            modelBuilder.Entity<RcdHistoricItem>().ForNpgsqlUseXminAsConcurrencyToken();

            var requestPropertyMap = new RequestPropertyMap(modelBuilder.Entity<RequestProperty>());
            modelBuilder.Entity<RequestProperty>().ForNpgsqlUseXminAsConcurrencyToken();
            
            var sourceMap = new SourceMap(modelBuilder.Entity<Source>());
            modelBuilder.Entity<Source>().ForNpgsqlUseXminAsConcurrencyToken();


            // MTM
            var currencySourceMap = new CurrencySourceMap(modelBuilder.Entity<CurrencySource>());
            modelBuilder.Entity<CurrencySource>().ForNpgsqlUseXminAsConcurrencyToken();


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
            try
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
            catch (Exception ex)
            {
                _logger.LogCritical("[NozomiDbContext]: " + ex);
            }
        }
    }
}
