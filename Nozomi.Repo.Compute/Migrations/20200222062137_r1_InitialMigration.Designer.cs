﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nozomi.Repo.Compute.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Compute.Migrations
{
    [DbContext(typeof(NozomiComputeDbContext))]
    [Migration("20200222062137_r1_InitialMigration")]
    partial class r1_InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Nozomi.Data.Models.Web.Compute", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedById")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DeletedById")
                        .HasColumnType("text");

                    b.Property<string>("Formula")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("Key")
                        .HasColumnType("text");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("text");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Guid")
                        .HasName("Compute_PK_Guid");

                    b.ToTable("Computes");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.ComputeExpression", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ComputeGuid")
                        .HasColumnType("uuid");

                    b.Property<string>("Expression")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(1);

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Guid")
                        .HasName("ComputeExpression_PK_Guid");

                    b.HasIndex("ComputeGuid");

                    b.ToTable("ComputeExpressions");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.ComputeValue", b =>
                {
                    b.Property<Guid>("ComputeGuid")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedById")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DeletedById")
                        .HasColumnType("text");

                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("ComputeGuid", "CreatedAt")
                        .HasName("ComputeValue_CK_ComputeGuid_CreatedAt");

                    b.HasAlternateKey("Guid")
                        .HasName("ComputeValue_AK_Guid");

                    b.ToTable("ComputeValues");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.SubCompute", b =>
                {
                    b.Property<Guid>("ChildComputeGuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParentComputeGuid")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreatedById")
                        .HasColumnType("text");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DeletedById")
                        .HasColumnType("text");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("text");

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("ChildComputeGuid", "ParentComputeGuid")
                        .HasName("SubCompute_CK_ChildComputeGuid_ParentComputeGuid");

                    b.HasIndex("ParentComputeGuid");

                    b.ToTable("SubComputes");
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.ComputeExpression", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Web.Compute", "Compute")
                        .WithMany("Expressions")
                        .HasForeignKey("ComputeGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.ComputeValue", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Web.Compute", "Compute")
                        .WithMany("Values")
                        .HasForeignKey("ComputeGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Nozomi.Data.Models.Web.SubCompute", b =>
                {
                    b.HasOne("Nozomi.Data.Models.Web.Compute", "ChildCompute")
                        .WithMany("ParentComputes")
                        .HasForeignKey("ChildComputeGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Nozomi.Data.Models.Web.Compute", "ParentCompute")
                        .WithMany("ChildComputes")
                        .HasForeignKey("ParentComputeGuid")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
