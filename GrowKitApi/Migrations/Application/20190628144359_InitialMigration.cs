using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GrowKitApi.Migrations.Application
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthenticationUser",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    NormalizedEmailAdress = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    AuthenticatorSecret = table.Column<byte[]>(nullable: true),
                    PreviousTimeWindow = table.Column<long>(nullable: false),
                    Flags = table.Column<short>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlantPresets",
                columns: table => new
                {
                    PresetId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Light = table.Column<int[]>(nullable: true),
                    Temperature = table.Column<int[]>(nullable: true),
                    Sunshine = table.Column<int[]>(nullable: true),
                    Moisture = table.Column<int[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantPresets", x => x.PresetId);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    NormalizedUsername = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_ApplicationUsers_AuthenticationUser_UserId",
                        column: x => x.UserId,
                        principalTable: "AuthenticationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorSticks",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    OwnerId = table.Column<long>(nullable: false),
                    MasterStickId = table.Column<long>(nullable: false),
                    ActivePresetId = table.Column<int>(nullable: true),
                    Light = table.Column<int>(nullable: false),
                    Moisture = table.Column<int>(nullable: false),
                    Temperature = table.Column<int>(nullable: false),
                    LightTime = table.Column<int>(nullable: false),
                    TimestampUpdate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorSticks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorSticks_PlantPresets_ActivePresetId",
                        column: x => x.ActivePresetId,
                        principalTable: "PlantPresets",
                        principalColumn: "PresetId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SensorSticks_SensorSticks_MasterStickId",
                        column: x => x.MasterStickId,
                        principalTable: "SensorSticks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SensorSticks_ApplicationUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SensorSticks_ActivePresetId",
                table: "SensorSticks",
                column: "ActivePresetId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorSticks_MasterStickId",
                table: "SensorSticks",
                column: "MasterStickId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorSticks_OwnerId",
                table: "SensorSticks",
                column: "OwnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SensorSticks");

            migrationBuilder.DropTable(
                name: "PlantPresets");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");

            migrationBuilder.DropTable(
                name: "AuthenticationUser");
        }
    }
}
