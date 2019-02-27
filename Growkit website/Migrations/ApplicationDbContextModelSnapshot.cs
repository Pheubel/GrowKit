﻿// <auto-generated />
using System;
using Growkit_website.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Growkit_website.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.2-servicing-10034")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("GrowkitDataModels.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("GrowkitDataModels.Friends", b =>
                {
                    b.Property<long>("RelationId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsConfirmed");

                    b.Property<Guid>("RequestRecieverId");

                    b.Property<Guid>("RequestSenderId");

                    b.HasKey("RelationId");

                    b.HasIndex("RequestRecieverId");

                    b.HasIndex("RequestSenderId");

                    b.ToTable("FriendRelations");
                });

            modelBuilder.Entity("GrowkitDataModels.GKHub", b =>
                {
                    b.Property<long>("IMSI")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("ApplicationUserId");

                    b.HasKey("IMSI");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("Hubs");
                });

            modelBuilder.Entity("GrowkitDataModels.GKSensorStick", b =>
                {
                    b.Property<long>("SensorId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ConnectedHubIMSI");

                    b.Property<short>("LightLevel");

                    b.Property<short>("PhLevel");

                    b.Property<string>("PlantName");

                    b.Property<short>("TemperatureLevel");

                    b.Property<short>("WaterLevel");

                    b.HasKey("SensorId");

                    b.HasIndex("ConnectedHubIMSI");

                    b.ToTable("SensorSticks");
                });

            modelBuilder.Entity("GrowkitDataModels.PlantPreset", b =>
                {
                    b.Property<int>("PresetId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EstimatedGrowthTime");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<short>("OptimalLightLevel");

                    b.Property<short>("OptimalPhLevel");

                    b.Property<short>("OptimalTemperatureLevel");

                    b.Property<short>("OptimalWaterLevel");

                    b.HasKey("PresetId");

                    b.ToTable("Presets");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>", b =>
                {
                    b.Property<Guid>("Id")
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

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName");

                    b.Property<Guid>("UserId");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<Guid>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasMaxLength(128);

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("GrowkitDataModels.Friends", b =>
                {
                    b.HasOne("GrowkitDataModels.ApplicationUser", "RequestReciever")
                        .WithMany()
                        .HasForeignKey("RequestRecieverId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GrowkitDataModels.ApplicationUser", "RequestSender")
                        .WithMany()
                        .HasForeignKey("RequestSenderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GrowkitDataModels.GKHub", b =>
                {
                    b.HasOne("GrowkitDataModels.ApplicationUser")
                        .WithMany("OwnedHubs")
                        .HasForeignKey("ApplicationUserId");
                });

            modelBuilder.Entity("GrowkitDataModels.GKSensorStick", b =>
                {
                    b.HasOne("GrowkitDataModels.GKHub", "ConnectedHub")
                        .WithMany("Sensors")
                        .HasForeignKey("ConnectedHubIMSI");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<System.Guid>", b =>
                {
                    b.HasOne("GrowkitDataModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<System.Guid>", b =>
                {
                    b.HasOne("GrowkitDataModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<System.Guid>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<System.Guid>")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GrowkitDataModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<System.Guid>", b =>
                {
                    b.HasOne("GrowkitDataModels.ApplicationUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
