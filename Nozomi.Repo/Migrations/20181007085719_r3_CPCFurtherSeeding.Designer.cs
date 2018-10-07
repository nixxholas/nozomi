﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nozomi.Repo.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    [DbContext(typeof(NozomiDbContext))]
    [Migration("20181007085719_r3_CPCFurtherSeeding")]
    partial class r3_CPCFurtherSeeding
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.3-rtm-32065")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.Currency", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abbrv")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<long>("CurrencySourceId");

                    b.Property<long>("CurrencyTypeId");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<long>("WalletTypeId");

                    b.HasKey("Id");

                    b.HasIndex("CurrencySourceId");

                    b.HasIndex("CurrencyTypeId");

                    b.ToTable("Currencies");

                    b.HasData(
                        new { Id = 1L, Abbrv = "USD", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, CurrencySourceId = 1L, CurrencyTypeId = 1L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "United States Dollar", WalletTypeId = 0L },
                        new { Id = 2L, Abbrv = "ETH", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, CurrencySourceId = 1L, CurrencyTypeId = 2L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "Ethereum", WalletTypeId = 1L },
                        new { Id = 3L, Abbrv = "KNC", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, CurrencySourceId = 1L, CurrencyTypeId = 2L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "Kyber Network Coin", WalletTypeId = 4L }
                    );
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.CurrencyPair", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("APIUrl")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<int>("CurrencyPairType");

                    b.Property<long>("CurrencySourceId");

                    b.Property<string>("DefaultComponent")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.HasKey("Id");

                    b.HasIndex("CurrencySourceId");

                    b.ToTable("CurrencyPairs");

                    b.HasData(
                        new { Id = 1L, APIUrl = "https://api.ethfinex.com/v2/ticker/tETHUSD", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, CurrencyPairType = 1, CurrencySourceId = 1L, DefaultComponent = "0", DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L },
                        new { Id = 2L, APIUrl = "https://api.ethfinex.com/v2/ticker/tKNCUSD", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, CurrencyPairType = 1, CurrencySourceId = 1L, DefaultComponent = "0", DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L }
                    );
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.CurrencyType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("TypeShortForm")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.HasKey("Id");

                    b.ToTable("CurrencyTypes");

                    b.HasData(
                        new { Id = 1L, CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "FIAT Cash", TypeShortForm = "FIAT" },
                        new { Id = 2L, CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "Cryptocurrency", TypeShortForm = "CRYPTO" }
                    );
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.PartialCurrencyPair", b =>
                {
                    b.Property<long>("CurrencyPairId");

                    b.Property<bool>("IsMain");

                    b.Property<long>("CurrencyId");

                    b.HasKey("CurrencyPairId", "IsMain");

                    b.HasIndex("CurrencyId");

                    b.ToTable("PartialCurrencyPairs");

                    b.HasData(
                        new { CurrencyPairId = 1L, IsMain = false, CurrencyId = 1L },
                        new { CurrencyPairId = 1L, IsMain = true, CurrencyId = 2L },
                        new { CurrencyPairId = 2L, IsMain = false, CurrencyId = 1L },
                        new { CurrencyPairId = 2L, IsMain = true, CurrencyId = 3L }
                    );
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.Source", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("APIDocsURL");

                    b.Property<string>("Abbreviation")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("Abbreviation");

                    b.ToTable("Sources");

                    b.HasData(
                        new { Id = 1L, APIDocsURL = "https://docs.bitfinex.com/docs/introduction", Abbreviation = "BFX", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "Bitfinex" },
                        new { Id = 2L, APIDocsURL = "None", Abbreviation = "HAKO", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "Coinhako" }
                    );
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.LoggingModels.RequestLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<string>("RawPayload");

                    b.Property<long>("RequestId");

                    b.Property<int>("Type");

                    b.HasKey("Id")
                        .HasName("RequestLog_PK_Id");

                    b.HasIndex("RequestId");

                    b.ToTable("RequestLogs");
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.Request", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<string>("DataPath");

                    b.Property<int>("Delay")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<int>("RequestType");

                    b.HasKey("Id")
                        .HasName("Request_PK_Id");

                    b.HasAlternateKey("Guid")
                        .HasName("Request_AK_Guid");

                    b.ToTable("Requests");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Request");
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.RequestComponent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<string>("QueryComponent");

                    b.Property<long>("RequestId");

                    b.HasKey("Id")
                        .HasName("RequestComponent_PK_Id");

                    b.HasIndex("RequestId");

                    b.ToTable("RequestComponents");

                    b.HasDiscriminator<string>("Discriminator").HasValue("RequestComponent");
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.RequestComponentDatum", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<long>("RequestComponentId");

                    b.Property<string>("Value")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("");

                    b.HasKey("Id")
                        .HasName("RequestComponenDatum_PK_Id");

                    b.HasIndex("RequestComponentId");

                    b.ToTable("RequestComponentData");
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.RequestProperty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("Key");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<long>("RequestId");

                    b.Property<int>("RequestPropertyType");

                    b.Property<string>("Value");

                    b.HasKey("Id")
                        .HasName("RequestProperty_PK_Id");

                    b.HasIndex("RequestId");

                    b.ToTable("RequestProperties");
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.CurrencyPairRequest", b =>
                {
                    b.HasBaseType("Nozomi.Data.WebModels.Request");

                    b.Property<long>("CurrencyPairId");

                    b.HasIndex("CurrencyPairId");

                    b.ToTable("CurrencyPairRequest");

                    b.HasDiscriminator().HasValue("CurrencyPairRequest");

                    b.HasData(
                        new { Id = 1L, CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, DataPath = "https://api.ethfinex.com/v2/ticker/tETHUSD", Delay = 5000, DeletedBy = 0L, Guid = new Guid("5b14895a-2ede-46ea-bf50-01b1c3ba71fe"), IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, RequestType = 0, CurrencyPairId = 1L },
                        new { Id = 2L, CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, DataPath = "https://api.ethfinex.com/v2/ticker/tKNCUSD", Delay = 5000, DeletedBy = 0L, Guid = new Guid("af646f80-f04e-4660-add3-43cf112b1b7d"), IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, RequestType = 0, CurrencyPairId = 2L }
                    );
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.CurrencyPairComponent", b =>
                {
                    b.HasBaseType("Nozomi.Data.WebModels.RequestComponent");

                    b.Property<int>("ComponentType");

                    b.Property<long>("CurrencyPairId");

                    b.HasIndex("CurrencyPairId");

                    b.ToTable("CurrencyPairComponent");

                    b.HasDiscriminator().HasValue("CurrencyPairComponent");

                    b.HasData(
                        new { Id = 1L, CreatedAt = new DateTime(2018, 10, 7, 16, 57, 19, 261, DateTimeKind.Local), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(2018, 10, 7, 16, 57, 19, 264, DateTimeKind.Local), ModifiedBy = 0L, QueryComponent = "1", RequestId = 1L, ComponentType = 1, CurrencyPairId = 1L },
                        new { Id = 2L, CreatedAt = new DateTime(2018, 10, 7, 16, 57, 19, 264, DateTimeKind.Local), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(2018, 10, 7, 16, 57, 19, 264, DateTimeKind.Local), ModifiedBy = 0L, QueryComponent = "0", RequestId = 1L, ComponentType = 2, CurrencyPairId = 1L },
                        new { Id = 3L, CreatedAt = new DateTime(2018, 10, 7, 16, 57, 19, 264, DateTimeKind.Local), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(2018, 10, 7, 16, 57, 19, 264, DateTimeKind.Local), ModifiedBy = 0L, QueryComponent = "0", RequestId = 2L, ComponentType = 1, CurrencyPairId = 2L }
                    );
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.Currency", b =>
                {
                    b.HasOne("Nozomi.Data.CurrencyModels.Source", "CurrencySource")
                        .WithMany("Currencies")
                        .HasForeignKey("CurrencySourceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nozomi.Data.CurrencyModels.CurrencyType", "CurrencyType")
                        .WithMany("Currencies")
                        .HasForeignKey("CurrencyTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.CurrencyPair", b =>
                {
                    b.HasOne("Nozomi.Data.CurrencyModels.Source", "CurrencySource")
                        .WithMany("CurrencyPairs")
                        .HasForeignKey("CurrencySourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.PartialCurrencyPair", b =>
                {
                    b.HasOne("Nozomi.Data.CurrencyModels.Currency", "Currency")
                        .WithMany("PartialCurrencyPairs")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nozomi.Data.CurrencyModels.CurrencyPair", "CurrencyPair")
                        .WithMany("PartialCurrencyPairs")
                        .HasForeignKey("CurrencyPairId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.LoggingModels.RequestLog", b =>
                {
                    b.HasOne("Nozomi.Data.WebModels.Request", "Request")
                        .WithMany("RequestLogs")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.RequestComponent", b =>
                {
                    b.HasOne("Nozomi.Data.WebModels.Request", "Request")
                        .WithMany("RequestComponents")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.RequestComponentDatum", b =>
                {
                    b.HasOne("Nozomi.Data.WebModels.RequestComponent", "RequestComponent")
                        .WithMany("RequestComponentData")
                        .HasForeignKey("RequestComponentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.RequestProperty", b =>
                {
                    b.HasOne("Nozomi.Data.WebModels.Request", "Request")
                        .WithMany("RequestProperties")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.CurrencyPairRequest", b =>
                {
                    b.HasOne("Nozomi.Data.CurrencyModels.CurrencyPair", "CurrencyPair")
                        .WithMany("CurrencyPairRequests")
                        .HasForeignKey("CurrencyPairId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.CurrencyPairComponent", b =>
                {
                    b.HasOne("Nozomi.Data.CurrencyModels.CurrencyPair", "CurrencyPair")
                        .WithMany("CurrencyPairComponents")
                        .HasForeignKey("CurrencyPairId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
