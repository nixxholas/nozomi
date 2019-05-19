using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nozomi.Base.Core;
using Nozomi.Data;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Logging;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Repo.BCL.Context;
using Nozomi.Repo.Data.Mappings.CurrencyModels;
using Nozomi.Repo.Data.Mappings.WebModels;
using Nozomi.Repo.Data.Mappings.WebModels.AnalyticalModels;
using Nozomi.Repo.Data.Mappings.WebModels.LoggingModels;
using Nozomi.Repo.Data.Mappings.WebModels.WebsocketModels;

namespace Nozomi.Repo.Data
{
    public class NozomiDbContext : DbContext, IDbContext
    {
        public DbSet<AnalysedComponent> AnalysedComponents { get; set; }
        public DbSet<AnalysedHistoricItem> AnalysedHistoricItems { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyRequest> CurrencyRequests { get; set; }
        public DbSet<CurrencyPair> CurrencyPairs { get; set; }
        public DbSet<CurrencyPairRequest> CurrencyPairRequests { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<CurrencyPairSourceCurrency> CurrencyCurrencyPairs { get; set; }
        public DbSet<CurrencySource> CurrencySources { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestComponent> RequestComponents { get; set; }
        public DbSet<RcdHistoricItem> RcdHistoricItems { get; set; }
        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<RequestProperty> RequestProperties { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<WebsocketRequest> WebsocketRequests { get; set; }
        public DbSet<WebsocketCommand> WebsocketCommands { get; set; }
        public DbSet<WebsocketCommandProperty> WebsocketCommandProperties { get; set; }
        
        public NozomiDbContext(DbContextOptions<NozomiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var analysedComponentMap = new AnalysedComponentMap(modelBuilder.Entity<AnalysedComponent>());
            var analysedHistoricItemMap = new AnalysedHistoricItemMap(modelBuilder.Entity<AnalysedHistoricItem>());
            var currencyMap = new CurrencyMap(modelBuilder.Entity<Currency>());
            var currencyRequestMap = new CurrencyRequestMap(modelBuilder.Entity<CurrencyRequest>());
            var currencyPairMap = new CurrencyPairMap(modelBuilder.Entity<CurrencyPair>());
            var currencyPairRequestMap = new CurrencyPairRequestMap(modelBuilder.Entity<CurrencyPairRequest>());
            var currencyTypeMap = new CurrencyTypeMap(modelBuilder.Entity<CurrencyType>());
            var currencyCurrencyPairMap = new CurrencyPairSourceCurrencyMap(modelBuilder.Entity<CurrencyPairSourceCurrency>());
            var currencySourceMap = new CurrencySourceMap(modelBuilder.Entity<CurrencySource>());
            var requestMap = new RequestMap(modelBuilder.Entity<Request>());
            var webSocketRequestMap = new WebsocketRequestMap(modelBuilder.Entity<WebsocketRequest>());
            var webSocketCommandMap = new WebsocketCommandMap(modelBuilder.Entity<WebsocketCommand>());
            var webSocketCommandPropertyMap = new WebsocketCommandPropertyMap(modelBuilder.Entity<WebsocketCommandProperty>());
            var requestComponentMap = new RequestComponentMap(modelBuilder.Entity<RequestComponent>());
            var rcdHistoricItemMap = new RcdHistoricItemMap(modelBuilder.Entity<RcdHistoricItem>());
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
