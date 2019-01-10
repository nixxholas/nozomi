﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nozomi.Repo.Identity.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nozomi.Repo.Identity.Migrations
{
    [DbContext(typeof(NozomiAuthContext))]
    partial class NozomiAuthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.ApiToken", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new Guid("7bd9669c-eaf0-40dc-bb9c-74929b170dc0"));

                    b.Property<string>("ApiKey")
                        .IsRequired();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt");

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("Label");

                    b.Property<DateTime>("LastAccessed")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2019, 1, 10, 14, 44, 50, 975, DateTimeKind.Local).AddTicks(2140));

                    b.Property<DateTime>("ModifiedAt");

                    b.Property<long>("ModifiedBy");

                    b.Property<string>("PublicKey")
                        .IsRequired();

                    b.Property<long>("UserId");

                    b.HasKey("Guid")
                        .HasName("ApiToken_PK_Guid");

                    b.HasIndex("UserId");

                    b.ToTable("ApiTokens");
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.Role", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("RoleClaims");
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<string>("StripeCustomerId");

                    b.Property<string>("StripeSourceId");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<uint>("xmin")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid");

                    b.HasKey("Id")
                        .HasName("User_PK_Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasName("User_Index_Email");

                    b.HasIndex("NormalizedEmail")
                        .IsUnique()
                        .HasName("User_Index_NormalizedEmail");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("User_Index_NormalizedUserName");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasName("User_Index_UserName");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.UserClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserClaims");
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.UserLogin", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<long>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("UserLogins");
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.UserRole", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<long>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.UserToken", b =>
                {
                    b.Property<long>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("UserTokens");
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Subscription.DevKey", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2019, 1, 10, 6, 44, 50, 982, DateTimeKind.Utc).AddTicks(810));

                    b.Property<long>("CreatedBy");

                    b.Property<DateTime?>("DeletedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(null);

                    b.Property<long>("DeletedBy");

                    b.Property<bool>("IsEnabled");

                    b.Property<string>("Key");

                    b.Property<DateTime>("ModifiedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(new DateTime(2019, 1, 10, 6, 44, 50, 982, DateTimeKind.Utc).AddTicks(1410));

                    b.Property<long>("ModifiedBy");

                    b.Property<long>("UserSubscriptionId");

                    b.HasKey("Id")
                        .HasName("DevKey_PK_Id");

                    b.HasIndex("UserSubscriptionId");

                    b.ToTable("DevKeys");
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Subscription.UserSubscription", b =>
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

                    b.Property<int>("PlanType")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0);

                    b.Property<string>("SubscriptionId")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(null);

                    b.Property<long>("UserId");

                    b.HasKey("Id")
                        .HasName("UserSubscription_PK_Id");

                    b.HasIndex("UserId", "DeletedAt")
                        .IsUnique()
                        .HasName("UserSubscription_Index_UserId_DeletedAt");

                    b.HasIndex("UserId", "SubscriptionId")
                        .IsUnique()
                        .HasName("UserSubscription_Index_UserId_SubscriptionId");

                    b.ToTable("UserSubscriptions");
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.ApiToken", b =>
                {
                    b.HasOne("Nozomi.Base.Identity.Models.Identity.User", "User")
                        .WithMany("ApiTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.RoleClaim", b =>
                {
                    b.HasOne("Nozomi.Base.Identity.Models.Identity.Role", "Role")
                        .WithMany("RoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.UserClaim", b =>
                {
                    b.HasOne("Nozomi.Base.Identity.Models.Identity.User", "User")
                        .WithMany("UserClaims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.UserLogin", b =>
                {
                    b.HasOne("Nozomi.Base.Identity.Models.Identity.User", "User")
                        .WithMany("UserLogins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.UserRole", b =>
                {
                    b.HasOne("Nozomi.Base.Identity.Models.Identity.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nozomi.Base.Identity.Models.Identity.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Identity.UserToken", b =>
                {
                    b.HasOne("Nozomi.Base.Identity.Models.Identity.User", "User")
                        .WithMany("UserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Subscription.DevKey", b =>
                {
                    b.HasOne("Nozomi.Base.Identity.Models.Subscription.UserSubscription", "UserSubscription")
                        .WithMany("DevKeys")
                        .HasForeignKey("UserSubscriptionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Nozomi.Base.Identity.Models.Subscription.UserSubscription", b =>
                {
                    b.HasOne("Nozomi.Base.Identity.Models.Identity.User", "User")
                        .WithMany("UserSubscriptions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
