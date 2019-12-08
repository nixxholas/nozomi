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
    [Migration("20191101093248_r10_SourceType")]
    partial class r10_SourceType
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Nozomi.Data.Models.Currency.Currency", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abbreviation")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<long>("CurrencyTypeId");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<string>("DenominationName");

                    b.Property<int>("Denominations")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("Description");

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("LogoPath")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("assets/svg/icons/question.svg");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Slug")
                        .IsRequired();

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("Currency_PK_Id");

                    b.HasIndex("CurrencyTypeId");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasName("Currency_Index_Slug");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Currency.CurrencyPair", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("APIUrl")
                        .IsRequired();

                    b.Property<string>("CounterCurrencyAbbrv")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<int>("CurrencyPairType");

                    b.Property<string>("DefaultComponent")
                        .IsRequired();

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("MainCurrencyAbbrv")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<long>("SourceId");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("CurrencyPair_PK_Id");

                    b.HasAlternateKey("MainCurrencyAbbrv", "CounterCurrencyAbbrv", "SourceId")
                        .HasName("CurrencyPair_AK_MainCurrency_CounterCurrency_Source");

                    b.HasIndex("SourceId");

                    b.ToTable("CurrencyPairs");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Currency.CurrencyProperty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<long>("CurrencyId");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<int>("Type");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("CurrencyProperty");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Currency.CurrencySource", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<long>("CurrencyId");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<long>("SourceId");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("CurrencySource_PK_Id");

                    b.HasIndex("SourceId");

                    b.HasIndex("CurrencyId", "SourceId")
                        .IsUnique()
                        .HasName("CurrencySource_CK_CurrencyId_SourceId");

                    b.ToTable("CurrencySources");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Currency.CurrencyType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("TypeShortForm")
                        .IsRequired()
                        .HasMaxLength(12);

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("CurrencyType_PK_Id");

                    b.ToTable("CurrencyTypes");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Currency.Source", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("APIDocsURL");

                    b.Property<string>("Abbreviation")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("Source_PK_Id");

                    b.HasIndex("Abbreviation")
                        .HasName("Source_Index_Abbreviation");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Currency.SourceType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Abbreviation")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("SourceType_Id_PK");

                    b.HasAlternateKey("Guid");

                    b.HasIndex("Abbreviation")
                        .IsUnique();

                    b.ToTable("SourceTypes");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.Analytical.AnalysedComponent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ComponentType")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<long?>("CurrencyId");

                    b.Property<long?>("CurrencyPairId");

                    b.Property<long?>("CurrencyTypeId");

                    b.Property<int>("Delay")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(86400000);

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<bool>("IsDenominated");

                    b.Property<bool>("IsEnabled");

                    b.Property<bool>("IsFailing")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<bool>("StoreHistoricals")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("UIFormatting");

                    b.Property<string>("Value");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("AnalysedComponent_PK_Id");

                    b.HasIndex("CurrencyId", "ComponentType")
                        .IsUnique()
                        .HasName("AnalysedComponent_Index_CurrencyId_ComponentType");

                    b.HasIndex("CurrencyPairId", "ComponentType")
                        .IsUnique()
                        .HasName("AnalysedComponent_Index_CurrencyPairId_ComponentType");

                    b.HasIndex("CurrencyTypeId", "ComponentType")
                        .IsUnique()
                        .HasName("AnalysedComponent_Index_CurrencyTypeId_ComponentType");

                    b.ToTable("AnalysedComponents");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.Analytical.AnalysedHistoricItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AnalysedComponentId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<DateTime>("HistoricDateTime");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("AnalysedHistoricItem_PK_Id");

                    b.HasIndex("AnalysedComponentId");

                    b.ToTable("AnalysedHistoricItems");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.RcdHistoricItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<DateTime>("HistoricDateTime");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<long>("RequestComponentId");

                    b.Property<string>("Value")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue("");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("RcdHistoricItem_PK_Id");

                    b.HasIndex("RequestComponentId");

                    b.ToTable("RcdHistoricItems");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.Request", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<long?>("CurrencyId");

                    b.Property<long?>("CurrencyPairId");

                    b.Property<long?>("CurrencyTypeId");

                    b.Property<string>("DataPath");

                    b.Property<int>("Delay")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<long>("FailureDelay")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(3600000L);

                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<int>("RequestType");

                    b.Property<int>("ResponseType")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("Request_PK_Id");

                    b.HasAlternateKey("Guid")
                        .HasName("Request_AK_Guid");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("CurrencyPairId");

                    b.HasIndex("CurrencyTypeId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.RequestComponent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AnomalyIgnorance")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<int>("ComponentType");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<string>("Identifier");

                    b.Property<bool>("IsDenominated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("QueryComponent");

                    b.Property<long>("RequestId");

                    b.Property<bool>("StoreHistoricals")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Value");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("RequestComponent_PK_Id");

                    b.HasIndex("RequestId", "ComponentType")
                        .IsUnique()
                        .HasName("RequestComponent_AK_RequestId_ComponentType");

                    b.ToTable("RequestComponents");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.RequestProperty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("Key");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<long>("RequestId");

                    b.Property<int>("RequestPropertyType");

                    b.Property<string>("Value");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("RequestProperty_PK_Id");

                    b.HasIndex("RequestId");

                    b.ToTable("RequestProperties");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.Websocket.WebsocketCommand", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CommandType");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<long>("Delay")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0L);

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Name");

                    b.Property<long>("RequestId");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("WebsocketCommand_PK_Id");

                    b.HasIndex("RequestId");

                    b.ToTable("WebsocketCommands");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.Websocket.WebsocketCommandProperty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CommandPropertyType");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("CreatedById");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<string>("DeletedById");

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(null);

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<string>("ModifiedById");

                    b.Property<string>("Value")
                        .IsRequired();

                    b.Property<long>("WebsocketCommandId");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("WebsocketCommandProperty_PK_Id");

                    b.HasIndex("WebsocketCommandId");

                    b.ToTable("WebsocketCommandProperties");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Currency.Currency", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Currency.CurrencyType", "CurrencyType")
                        .WithMany("Currencies")
                        .HasForeignKey("CurrencyTypeId")
                        .HasConstraintName("CurrencyType_Currencies_Constraint")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.Models.Currency.CurrencyPair", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Currency.Source", "Source")
                        .WithMany("CurrencyPairs")
                        .HasForeignKey("SourceId")
                        .HasConstraintName("Source_CurrencyPairs_Constraint")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.Models.Currency.CurrencyProperty", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Currency.Currency", "Currency")
                        .WithMany("CurrencyProperties")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.Models.Currency.CurrencySource", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Currency.Currency", "Currency")
                        .WithMany("CurrencySources")
                        .HasForeignKey("CurrencyId")
                        .HasConstraintName("CurrencySource_Currency_Constraint")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Nozomi.Data.Models.Currency.Source", "Source")
                        .WithMany("SourceCurrencies")
                        .HasForeignKey("SourceId")
                        .HasConstraintName("CurrencySource_Source_Constraint")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.Analytical.AnalysedComponent", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Currency.Currency", "Currency")
                        .WithMany("AnalysedComponents")
                        .HasForeignKey("CurrencyId")
                        .HasConstraintName("Currency_AnalysedComponents_Constraint")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nozomi.Data.Models.Currency.CurrencyPair", "CurrencyPair")
                        .WithMany("AnalysedComponents")
                        .HasForeignKey("CurrencyPairId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nozomi.Data.Models.Currency.CurrencyType", "CurrencyType")
                        .WithMany("AnalysedComponents")
                        .HasForeignKey("CurrencyTypeId")
                        .HasConstraintName("CurrencyType_AnalysedComponents_Constraint")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.Analytical.AnalysedHistoricItem", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Web.Analytical.AnalysedComponent", "AnalysedComponent")
                        .WithMany("AnalysedHistoricItems")
                        .HasForeignKey("AnalysedComponentId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.RcdHistoricItem", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Web.RequestComponent", "RequestComponent")
                        .WithMany("RcdHistoricItems")
                        .HasForeignKey("RequestComponentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.Request", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Currency.Currency", "Currency")
                        .WithMany("Requests")
                        .HasForeignKey("CurrencyId")
                        .HasConstraintName("Currencies_CurrencyRequests_Constraint")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nozomi.Data.Models.Currency.CurrencyPair", "CurrencyPair")
                        .WithMany("Requests")
                        .HasForeignKey("CurrencyPairId")
                        .HasConstraintName("CurrencyPair_CurrencyPairRequest_Constraint");

                    b.HasOne("Nozomi.Data.Models.Currency.CurrencyType", "CurrencyType")
                        .WithMany("Requests")
                        .HasForeignKey("CurrencyTypeId")
                        .HasConstraintName("CurrencyType_Request_Constraint");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.RequestComponent", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Web.Request", "Request")
                        .WithMany("RequestComponents")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.RequestProperty", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Web.Request", "Request")
                        .WithMany("RequestProperties")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.Websocket.WebsocketCommand", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Web.Request", "Request")
                        .WithMany("WebsocketCommands")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.Websocket.WebsocketCommandProperty", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Web.Websocket.WebsocketCommand", "WebsocketCommand")
                        .WithMany("WebsocketCommandProperties")
                        .HasForeignKey("WebsocketCommandId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
