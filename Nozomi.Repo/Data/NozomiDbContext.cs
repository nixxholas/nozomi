using Microsoft.EntityFrameworkCore;
using Nozomi.Data.CurrencyModels;
using System;
using System.Collections.Generic;
using System.Text;
using Nozomi.Data.WebModels;
using Nozomi.Repo.Data.Mappings.CurrencyModels;
using Nozomi.Repo.Data.Mappings.WebModels;

namespace Nozomi.Repo.Data
{
    public class NozomiDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyPair> CurrencyPairs { get; set; }
        public DbSet<CurrencyPairComponent> CurrencyPairComponents { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<PartialCurrencyPair> PartialCurrencyPairs { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Source> Sources { get; set; }

        public NozomiDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var currencyMap = new CurrencyMap(modelBuilder.Entity<Currency>());
            var currencyPairMap = new CurrencyPairMap(modelBuilder.Entity<CurrencyPair>());
            var currencyPairComponentMap = new CurrencyPairComponentMap(modelBuilder.Entity<CurrencyPairComponent>());
            var currencyTypeMap = new CurrencyTypeMap(modelBuilder.Entity<CurrencyType>());
            var partialCurrencyPairMap = new PartialCurrencyPairMap(modelBuilder.Entity<PartialCurrencyPair>());
            var requestMap = new RequestMap(modelBuilder.Entity<Request>());
            var sourceMap = new SourceMap(modelBuilder.Entity<Source>());
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
