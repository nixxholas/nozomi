﻿using Microsoft.EntityFrameworkCore;
using Nozomi.Data.CurrencyModels;
using Nozomi.Repo.Data.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nozomi.Repo.Data
{
    public class NozomiDbContext : DbContext
    {
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrencyPair> CurrencyPairs { get; set; }
        public DbSet<CurrencyPairComponent> CurrencyPairComponents { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<PartialCurrencyPair> PartialCurrencyPairs { get; set; }

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
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
