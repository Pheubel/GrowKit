﻿// <auto-generated />
using GrowKitApi.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GrowKitApi.Migrations.Application
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("GrowKitApi.Entities.ApplicationUser", b =>
                {
                    b.Property<long>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("NormalizedUsername");

                    b.Property<string>("Username");

                    b.HasKey("UserId");

                    b.ToTable("ApplicationUsers");
                });

            modelBuilder.Entity("GrowKitApi.Entities.GrowKitStick", b =>
                {
                    b.Property<long>("IMSI")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Light");

                    b.Property<int>("LightTime");

                    b.Property<long>("MasterStickId");

                    b.Property<int>("Moisture");

                    b.Property<long>("OwnerId");

                    b.Property<int>("Temperature");

                    b.Property<int>("TimestampUpdate");

                    b.HasKey("IMSI");

                    b.HasIndex("MasterStickId");

                    b.HasIndex("OwnerId");

                    b.ToTable("SensorSticks");
                });

            modelBuilder.Entity("GrowKitApi.Entities.PlantPreset", b =>
                {
                    b.Property<int>("PresetId")
                        .ValueGeneratedOnAdd();

                    b.HasKey("PresetId");

                    b.ToTable("Presets");
                });

            modelBuilder.Entity("GrowKitApi.Entities.GrowKitStick", b =>
                {
                    b.HasOne("GrowKitApi.Entities.GrowKitStick", "MasterStick")
                        .WithMany()
                        .HasForeignKey("MasterStickId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GrowKitApi.Entities.ApplicationUser", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
