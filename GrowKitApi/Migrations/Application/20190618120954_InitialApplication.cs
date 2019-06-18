using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace GrowKitApi.Migrations.Application
{
    public partial class InitialApplication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUsers",
                columns: table => new
                {
                    UserId = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    Username = table.Column<string>(nullable: true),
                    NormalizedUsername = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Presets",
                columns: table => new
                {
                    PresetId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Presets", x => x.PresetId);
                });

            migrationBuilder.CreateTable(
                name: "SensorSticks",
                columns: table => new
                {
                    IMSI = table.Column<long>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    OwnerId = table.Column<long>(nullable: false),
                    MasterStickId = table.Column<long>(nullable: false),
                    Light = table.Column<int>(nullable: false),
                    Moisture = table.Column<int>(nullable: false),
                    Temperature = table.Column<int>(nullable: false),
                    LightTime = table.Column<int>(nullable: false),
                    TimestampUpdate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorSticks", x => x.IMSI);
                    table.ForeignKey(
                        name: "FK_SensorSticks_SensorSticks_MasterStickId",
                        column: x => x.MasterStickId,
                        principalTable: "SensorSticks",
                        principalColumn: "IMSI",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SensorSticks_ApplicationUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "ApplicationUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "Presets");

            migrationBuilder.DropTable(
                name: "SensorSticks");

            migrationBuilder.DropTable(
                name: "ApplicationUsers");
        }
    }
}
