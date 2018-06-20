﻿using Microsoft.EntityFrameworkCore;
using Nozomi.Data.CurrencyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Counter.SDK.SharedModels;
using Nozomi.Data.WebModels;
using Nozomi.Repo.Data.Mappings.CurrencyModels;
using Nozomi.Repo.Data.Mappings.WebModels;

namespace Nozomi.Repo.Data
{
    public class NozomiDbContext : DbContext, IDbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyPair> CurrencyPairs { get; set; }
        public DbSet<CurrencyPairComponent> CurrencyPairComponents { get; set; }
        public DbSet<CurrencyPairRequest> CurrencyPairRequests { get; set; }
        public DbSet<CurrencyPairRequestComponent> CurrencyPairRequestComponents { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<PartialCurrencyPair> PartialCurrencyPairs { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<RequestComponent> RequestComponents { get; set; }
        public DbSet<RequestProperty> RequestProperties { get; set; }
        public DbSet<Source> Sources { get; set; }

        public NozomiDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var currencyMap = new CurrencyMap(modelBuilder.Entity<Currency>());
            var currencyPairMap = new CurrencyPairMap(modelBuilder.Entity<CurrencyPair>());
            var currencyPairComponentMap = new CurrencyPairComponentMap(modelBuilder.Entity<CurrencyPairComponent>());
            var currencyPairRequestMap = new CurrencyPairRequestMap(modelBuilder.Entity<CurrencyPairRequest>());
            var currencyPairRequestComponentMap = new CurrencyPairRequestComponentMap(modelBuilder.Entity<CurrencyPairRequestComponent>());
            var currencyTypeMap = new CurrencyTypeMap(modelBuilder.Entity<CurrencyType>());
            var partialCurrencyPairMap = new PartialCurrencyPairMap(modelBuilder.Entity<PartialCurrencyPair>());
            var requestMap = new RequestMap(modelBuilder.Entity<Request>());
            var requestComponentMap = new RequestComponentMap(modelBuilder.Entity<RequestComponent>());
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
        
        private void AddTimestamps(long userId = 0)
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
