using Microsoft.EntityFrameworkCore;
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

        public NozomiDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var currencyMap = new CurrencyMap(modelBuilder.Entity<Currency>());

            base.OnModelCreating(modelBuilder);
        }
    }
}
