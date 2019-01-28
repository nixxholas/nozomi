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
    [Migration("20190127125346_r2_WebsocketRequest")]
    partial class r2_WebsocketRequest
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
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

                    b.HasKey("Id")
                        .HasName("Currency_PK_Id");

                    b.HasIndex("CurrencySourceId");

                    b.HasIndex("CurrencyTypeId");

                    b.ToTable("Currencies");
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

                    b.HasKey("Id")
                        .HasName("CurrencyPair_PK_Id");

                    b.HasIndex("CurrencySourceId");

                    b.ToTable("CurrencyPairs");
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

                    b.HasKey("Id")
                        .HasName("CurrencyType_PK_Id");

                    b.ToTable("CurrencyTypes");
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.PartialCurrencyPair", b =>
                {
                    b.Property<long>("CurrencyPairId");

                    b.Property<bool>("IsMain");

                    b.Property<long>("CurrencyId");

                    b.HasKey("CurrencyPairId", "IsMain")
                        .HasName("PartialCurrencyPair_CK_CurrencyPairId_IsMain");

                    b.HasIndex("CurrencyId");

                    b.ToTable("PartialCurrencyPairs");
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

                    b.HasKey("Id")
                        .HasName("Source_PK_Id");

                    b.HasIndex("Abbreviation")
                        .HasName("Source_Index_Abbreviation");

                    b.ToTable("Sources");
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

            modelBuilder.Entity("Nozomi.Data.WebModels.RequestComponent", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ComponentType");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<string>("Identifier");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<string>("QueryComponent");

                    b.Property<long>("RequestId");

                    b.HasKey("Id")
                        .HasName("RequestComponent_PK_Id");

                    b.HasIndex("RequestId", "ComponentType")
                        .IsUnique()
                        .HasName("RequestComponent_AK_RequestId_ComponentType");

                    b.ToTable("RequestComponents");
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
                        .HasName("RequestComponentDatum_PK_Id");

                    b.HasIndex("RequestComponentId")
                        .IsUnique();

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

            modelBuilder.Entity("Nozomi.Data.WebModels.WebsocketModels.WebsocketCommand", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CommandType");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<long>("Delay");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<string>("Name");

                    b.Property<long>("WebsocketRequestId");

                    b.HasKey("Id");

                    b.HasIndex("WebsocketRequestId");

                    b.ToTable("WebsocketCommands");
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.WebsocketModels.WebsocketCommandProperty", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CommandPropertyType");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<string>("Value");

                    b.Property<long>("WebsocketCommandId");

                    b.HasKey("Id");

                    b.HasIndex("WebsocketCommandId");

                    b.ToTable("WebsocketCommandProperties");
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.CurrencyPairRequest", b =>
                {
                    b.HasBaseType("Nozomi.Data.WebModels.Request");

                    b.Property<long>("CurrencyPairId");

                    b.HasIndex("CurrencyPairId");

                    b.HasDiscriminator().HasValue("CurrencyPairRequest");
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.WebsocketModels.WebsocketRequest", b =>
                {
                    b.HasBaseType("Nozomi.Data.WebModels.Request");

                    b.Property<long>("CurrencyPairId")
                        .HasColumnName("WebsocketRequest_CurrencyPairId");

                    b.HasIndex("CurrencyPairId");

                    b.HasDiscriminator().HasValue("WebsocketRequest");
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.Currency", b =>
                {
                    b.HasOne("Nozomi.Data.CurrencyModels.Source", "CurrencySource")
                        .WithMany("Currencies")
                        .HasForeignKey("CurrencySourceId")
                        .HasConstraintName("Source_Currencies_Constraint")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nozomi.Data.CurrencyModels.CurrencyType", "CurrencyType")
                        .WithMany("Currencies")
                        .HasForeignKey("CurrencyTypeId")
                        .HasConstraintName("CurrencyType_Currencies_Constraint")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.CurrencyPair", b =>
                {
                    b.HasOne("Nozomi.Data.CurrencyModels.Source", "CurrencySource")
                        .WithMany("CurrencyPairs")
                        .HasForeignKey("CurrencySourceId")
                        .HasConstraintName("Source_CurrencyPairs_Constraint")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.CurrencyModels.PartialCurrencyPair", b =>
                {
                    b.HasOne("Nozomi.Data.CurrencyModels.Currency", "Currency")
                        .WithMany("PartialCurrencyPairs")
                        .HasForeignKey("CurrencyId")
                        .HasConstraintName("PartialCurrencyPairs_Currency_Constraint")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nozomi.Data.CurrencyModels.CurrencyPair", "CurrencyPair")
                        .WithMany("PartialCurrencyPairs")
                        .HasForeignKey("CurrencyPairId")
                        .HasConstraintName("PartialCurrencyPairs_CurrencyPair_Constraint")
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
                        .WithOne("RequestComponentDatum")
                        .HasForeignKey("Nozomi.Data.WebModels.RequestComponentDatum", "RequestComponentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.RequestProperty", b =>
                {
                    b.HasOne("Nozomi.Data.WebModels.Request", "Request")
                        .WithMany("RequestProperties")
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.WebsocketModels.WebsocketCommand", b =>
                {
                    b.HasOne("Nozomi.Data.WebModels.WebsocketModels.WebsocketRequest", "WebsocketRequest")
                        .WithMany("WebsocketCommands")
                        .HasForeignKey("WebsocketRequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.WebsocketModels.WebsocketCommandProperty", b =>
                {
                    b.HasOne("Nozomi.Data.WebModels.WebsocketModels.WebsocketCommand", "WebsocketCommand")
                        .WithMany("WebsocketCommandProperties")
                        .HasForeignKey("WebsocketCommandId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.CurrencyPairRequest", b =>
                {
                    b.HasOne("Nozomi.Data.CurrencyModels.CurrencyPair", "CurrencyPair")
                        .WithMany("CurrencyPairRequests")
                        .HasForeignKey("CurrencyPairId")
                        .HasConstraintName("CurrencyPair_CurrencyPairRequest_Constraint")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Data.WebModels.WebsocketModels.WebsocketRequest", b =>
                {
                    b.HasOne("Nozomi.Data.CurrencyModels.CurrencyPair", "CurrencyPair")
                        .WithMany("WebsocketRequests")
                        .HasForeignKey("CurrencyPairId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
