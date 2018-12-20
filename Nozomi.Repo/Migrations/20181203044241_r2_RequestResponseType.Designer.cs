﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nozomi.Data.WebModels;
using Nozomi.Repo.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Migrations
{
    [DbContext(typeof(NozomiDbContext))]
    [Migration("20181203044241_r2_RequestResponseType")]
    partial class r2_RequestResponseType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Nozomi.Base.Domain.CurrencyModels.Currency", b =>
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
                        new { Id = 3L, Abbrv = "KNC", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, CurrencySourceId = 1L, CurrencyTypeId = 2L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "Kyber Network Coin", WalletTypeId = 4L },
                        new { Id = 4L, Abbrv = "KNC", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, CurrencySourceId = 3L, CurrencyTypeId = 2L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "Kyber Network Coin", WalletTypeId = 4L },
                        new { Id = 5L, Abbrv = "ETH", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, CurrencySourceId = 3L, CurrencyTypeId = 2L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "Ethereum", WalletTypeId = 1L }
                    );
                });

            modelBuilder.Entity("Nozomi.Base.Domain.CurrencyModels.CurrencyPair", b =>
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
                        new { Id = 2L, APIUrl = "https://api.ethfinex.com/v2/ticker/tKNCUSD", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, CurrencyPairType = 1, CurrencySourceId = 1L, DefaultComponent = "0", DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L },
                        new { Id = 3L, APIUrl = "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, CurrencyPairType = 1, CurrencySourceId = 3L, DefaultComponent = "askPrice", DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L }
                    );
                });

            modelBuilder.Entity("Nozomi.Base.Domain.CurrencyModels.CurrencyType", b =>
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

            modelBuilder.Entity("Nozomi.Base.Domain.CurrencyModels.PartialCurrencyPair", b =>
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
                        new { CurrencyPairId = 2L, IsMain = true, CurrencyId = 3L },
                        new { CurrencyPairId = 3L, IsMain = true, CurrencyId = 4L },
                        new { CurrencyPairId = 3L, IsMain = false, CurrencyId = 5L }
                    );
                });

            modelBuilder.Entity("Nozomi.Base.Domain.CurrencyModels.Source", b =>
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
                        new { Id = 2L, APIDocsURL = "None", Abbreviation = "HAKO", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "Coinhako" },
                        new { Id = 3L, APIDocsURL = "https://github.com/binance-exchange/binance-official-api-docs/blob/master/rest-api.md", Abbreviation = "BNA", CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, Name = "Binance" }
                    );
                });

            modelBuilder.Entity("Nozomi.Base.Domain.WebModels.LoggingModels.RequestLog", b =>
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

            modelBuilder.Entity("Nozomi.Base.Domain.WebModels.Request", b =>
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

                    b.Property<int>("ResponseType")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.HasKey("Id")
                        .HasName("Request_PK_Id");

                    b.HasAlternateKey("Guid")
                        .HasName("Request_AK_Guid");

                    b.ToTable("Requests");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Request");
                });

            modelBuilder.Entity("Nozomi.Base.Domain.WebModels.RequestComponent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ComponentType");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<string>("QueryComponent");

                    b.Property<long>("RequestComponentDatumId");

                    b.Property<long>("RequestId");

                    b.HasKey("Id")
                        .HasName("RequestComponent_PK_Id");

                    b.HasIndex("RequestId", "ComponentType")
                        .IsUnique()
                        .HasName("RequestComponent_AK_RequestId_ComponentType");

                    b.ToTable("RequestComponents");

                    b.HasData(
                        new { Id = 1L, ComponentType = 1, CreatedAt = new DateTime(2018, 12, 3, 12, 42, 40, 707, DateTimeKind.Local), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), ModifiedBy = 0L, QueryComponent = "1", RequestComponentDatumId = 0L, RequestId = 1L },
                        new { Id = 2L, ComponentType = 2, CreatedAt = new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), ModifiedBy = 0L, QueryComponent = "0", RequestComponentDatumId = 0L, RequestId = 1L },
                        new { Id = 3L, ComponentType = 1, CreatedAt = new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), ModifiedBy = 0L, QueryComponent = "0", RequestComponentDatumId = 0L, RequestId = 2L },
                        new { Id = 4L, ComponentType = 1, CreatedAt = new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), ModifiedBy = 0L, QueryComponent = "askPrice", RequestComponentDatumId = 0L, RequestId = 3L },
                        new { Id = 5L, ComponentType = 2, CreatedAt = new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), CreatedBy = 0L, DeletedBy = 0L, IsEnabled = true, ModifiedAt = new DateTime(2018, 12, 3, 12, 42, 40, 710, DateTimeKind.Local), ModifiedBy = 0L, QueryComponent = "bidPrice", RequestComponentDatumId = 0L, RequestId = 3L }
                    );
                });

            modelBuilder.Entity("Nozomi.Base.Domain.WebModels.RequestComponentDatum", b =>
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
                        .HasName("RequestComponentDatum_PK_Id");

                    b.HasIndex("RequestComponentId")
                        .IsUnique();

                    b.ToTable("RequestComponentData");
                });

            modelBuilder.Entity("Nozomi.Base.Domain.WebModels.RequestProperty", b =>
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

            modelBuilder.Entity("Nozomi.Base.Domain.WebModels.CurrencyPairRequest", b =>
                {
                    b.HasBaseType("Nozomi.Base.Domain.WebModels.Request");

                    b.Property<long>("CurrencyPairId");

                    b.HasIndex("CurrencyPairId");

                    b.ToTable("CurrencyPairRequest");

                    b.HasDiscriminator().HasValue("CurrencyPairRequest");

                    b.HasData(
                        new { Id = 1L, CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, DataPath = "https://api.ethfinex.com/v2/ticker/tETHUSD", Delay = 5000, DeletedBy = 0L, Guid = new Guid("8fd9be01-afec-4e5c-8237-d4c16276f5bf"), IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, RequestType = 0, ResponseType = 0, CurrencyPairId = 1L },
                        new { Id = 2L, CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, DataPath = "https://api.ethfinex.com/v2/ticker/tKNCUSD", Delay = 5000, DeletedBy = 0L, Guid = new Guid("b9dfa033-bf21-4fa0-99fa-676d161989ad"), IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, RequestType = 0, ResponseType = 0, CurrencyPairId = 2L },
                        new { Id = 3L, CreatedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), CreatedBy = 0L, DataPath = "https://api.binance.com/api/v3/ticker/bookTicker?symbol=KNCETH", Delay = 5000, DeletedBy = 0L, Guid = new Guid("4a8a2493-62d5-4719-bb94-b18d4a80bfe6"), IsEnabled = true, ModifiedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), ModifiedBy = 0L, RequestType = 0, ResponseType = 0, CurrencyPairId = 3L }
                    );
                });

            modelBuilder.Entity("Nozomi.Base.Domain.CurrencyModels.Currency", b =>
                {
                    b.HasOne("Nozomi.Base.Domain.CurrencyModels.Source", "CurrencySource")
                        .WithMany("Currencies")
                        .HasForeignKey("CurrencySourceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nozomi.Base.Domain.CurrencyModels.CurrencyType", "CurrencyType")
                        .WithMany("Currencies")
                        .HasForeignKey("CurrencyTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Domain.CurrencyModels.CurrencyPair", b =>
                {
                    b.HasOne("Nozomi.Base.Domain.CurrencyModels.Source", "CurrencySource")
                        .WithMany("CurrencyPairs")
                        .HasForeignKey("CurrencySourceId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Domain.CurrencyModels.PartialCurrencyPair", b =>
                {
                    b.HasOne("Nozomi.Base.Domain.CurrencyModels.Currency", "Currency")
                        .WithMany("PartialCurrencyPairs")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nozomi.Base.Domain.CurrencyModels.CurrencyPair", "CurrencyPair")
                        .WithMany("PartialCurrencyPairs")
                        .HasForeignKey("CurrencyPairId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Domain.WebModels.LoggingModels.RequestLog", b =>
                {
                    b.HasOne("Nozomi.Base.Domain.WebModels.Request", "Request")
                        .WithMany("RequestLogs")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Domain.WebModels.RequestComponent", b =>
                {
                    b.HasOne("Nozomi.Base.Domain.WebModels.Request", "Request")
                        .WithMany("RequestComponents")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Domain.WebModels.RequestComponentDatum", b =>
                {
                    b.HasOne("Nozomi.Base.Domain.WebModels.RequestComponent", "RequestComponent")
                        .WithOne("RequestComponentDatum")
                        .HasForeignKey("Nozomi.Base.Domain.WebModels.RequestComponentDatum", "RequestComponentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Domain.WebModels.RequestProperty", b =>
                {
                    b.HasOne("Nozomi.Base.Domain.WebModels.Request", "Request")
                        .WithMany("RequestProperties")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Domain.WebModels.CurrencyPairRequest", b =>
                {
                    b.HasOne("Nozomi.Base.Domain.CurrencyModels.CurrencyPair", "CurrencyPair")
                        .WithMany("CurrencyPairRequests")
                        .HasForeignKey("CurrencyPairId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
