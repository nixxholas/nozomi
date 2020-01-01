using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nozomi.Base.BCL.Helpers.Mapping;
using Nozomi.Data.Models.Currency;

namespace Nozomi.Repo.Data.Mappings.CurrencyModels
{
    public class CurrencyPairMap : BaseMap<CurrencyPair>
    {
        public CurrencyPairMap(EntityTypeBuilder<CurrencyPair> entityTypeBuilder) : base(entityTypeBuilder)
        {
            entityTypeBuilder.HasKey(cp => cp.Id).HasName("CurrencyPair_PK_Id");
            entityTypeBuilder.Property(cp => cp.Id).ValueGeneratedOnAdd();

            entityTypeBuilder.Property(e => e.Guid).ValueGeneratedOnAdd().HasDefaultValueSql("uuid_generate_v4()");
            entityTypeBuilder.HasIndex(e => e.Guid).IsUnique();

            entityTypeBuilder.HasIndex(cp => new
            {
                cp.MainTicker, 
                cp.CounterTicker,
                cp.SourceId
            });

            entityTypeBuilder.Property(cp => cp.MainTicker)
                .HasConversion(val => val.ToUpperInvariant(), 
                    val => val);
            entityTypeBuilder.Property(cp => cp.CounterTicker)
                .HasConversion(val => val.ToUpperInvariant(), 
                    val => val);

            entityTypeBuilder.Property(cp => cp.APIUrl).IsRequired();
            entityTypeBuilder.Property(cp => cp.DefaultComponent).IsRequired();

            entityTypeBuilder.HasOne(cp => cp.Source).WithMany(cs => cs.CurrencyPairs)
                .HasForeignKey(cp => cp.SourceId)
                .HasConstraintName("CurrencyPairs_CurrencySource_Constraint");
            entityTypeBuilder.HasMany(cp => cp.AnalysedComponents).WithOne(ac => ac.CurrencyPair)
                .HasForeignKey(ac => ac.CurrencyPairId).OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            entityTypeBuilder.HasMany(cp => cp.Requests).WithOne(r => r.CurrencyPair)
                .HasForeignKey(r => r.CurrencyPairId)
                .HasConstraintName("CurrencyPair_CurrencyPairRequest_Constraint")
                .IsRequired(false);
        }
    }
}