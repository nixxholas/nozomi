using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nozomi.Base.BCL;
using Nozomi.Data.Models.Currency;
using Nozomi.Data.Models.Web;
using Nozomi.Data.Models.Web.Analytical;
using Nozomi.Data.Models.Web.Websocket;
using Nozomi.Repo.Data.Mappings.CurrencyModels;
using Nozomi.Repo.Data.Mappings.WebModels;
using Nozomi.Repo.Data.Mappings.WebModels.AnalyticalModels;
using Nozomi.Repo.Data.Mappings.WebModels.WebsocketModels;

namespace Nozomi.Repo.Data
{
    public class NozomiDbContext : DbContext
    {
        public DbSet<AnalysedComponent> AnalysedComponents { get; set; }
        public DbSet<AnalysedHistoricItem> AnalysedHistoricItems { get; set; }
        public DbSet<ComponentType> ComponentTypes { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyPair> CurrencyPairs { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<CurrencySource> CurrencySources { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Component> Components { get; set; }
        public DbSet<ComponentHistoricItem> ComponentHistoricItems { get; set; }
        public DbSet<RequestProperty> RequestProperties { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<SourceType> SourceTypes { get; set; }
        public DbSet<WebsocketCommand> WebsocketCommands { get; set; }
        public DbSet<WebsocketCommandProperty> WebsocketCommandProperties { get; set; }
        
        public NozomiDbContext(DbContextOptions<NozomiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.UseIdentityColumns();
            
            var analysedComponentMap = new AnalysedComponentMap(modelBuilder.Entity<AnalysedComponent>());
            modelBuilder.Entity<AnalysedComponent>().UseXminAsConcurrencyToken();

            var analysedHistoricItemMap = new AnalysedHistoricItemMap(modelBuilder.Entity<AnalysedHistoricItem>());
            modelBuilder.Entity<AnalysedHistoricItem>().UseXminAsConcurrencyToken();

            var componentTypeMap = new ComponentTypeMap(modelBuilder.Entity<ComponentType>());
            modelBuilder.Entity<ComponentType>().UseXminAsConcurrencyToken();

            var currencyMap = new CurrencyMap(modelBuilder.Entity<Currency>());
            modelBuilder.Entity<Currency>().UseXminAsConcurrencyToken();

            var currencyPairMap = new CurrencyPairMap(modelBuilder.Entity<CurrencyPair>());
            modelBuilder.Entity<CurrencyPair>().UseXminAsConcurrencyToken();

            var currencyTypeMap = new CurrencyTypeMap(modelBuilder.Entity<CurrencyType>());
            modelBuilder.Entity<CurrencyType>().UseXminAsConcurrencyToken();

            var requestMap = new RequestMap(modelBuilder.Entity<Request>());
            modelBuilder.Entity<Request>().UseXminAsConcurrencyToken();

            var webSocketCommandMap = new WebsocketCommandMap(modelBuilder.Entity<WebsocketCommand>());
            modelBuilder.Entity<WebsocketCommand>().UseXminAsConcurrencyToken();

            var webSocketCommandPropertyMap = new WebsocketCommandPropertyMap(modelBuilder.Entity<WebsocketCommandProperty>());
            modelBuilder.Entity<WebsocketCommandProperty>().UseXminAsConcurrencyToken();

            var requestComponentMap = new ComponentMap(modelBuilder.Entity<Component>());
            modelBuilder.Entity<Component>().UseXminAsConcurrencyToken();

            var rcdHistoricItemMap = new ComponentHistoricItemMap(modelBuilder.Entity<ComponentHistoricItem>());
            modelBuilder.Entity<ComponentHistoricItem>().UseXminAsConcurrencyToken();

            var requestPropertyMap = new RequestPropertyMap(modelBuilder.Entity<RequestProperty>());
            modelBuilder.Entity<RequestProperty>().UseXminAsConcurrencyToken();
            
            var sourceMap = new SourceMap(modelBuilder.Entity<Source>());
            modelBuilder.Entity<Source>().UseXminAsConcurrencyToken();

            var sourceTypeMap = new SourceTypeMap(modelBuilder.Entity<SourceType>());
            modelBuilder.Entity<SourceType>().UseXminAsConcurrencyToken();

            // MTM
            var currencySourceMap = new CurrencySourceMap(modelBuilder.Entity<CurrencySource>());
            modelBuilder.Entity<CurrencySource>().UseXminAsConcurrencyToken();
            
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
