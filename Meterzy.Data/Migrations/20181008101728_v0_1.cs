using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Meterzy.Data.Migrations
{
    public partial class v0_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EmailHash = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tariff",
                columns: table => new
                {
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AppUserId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tariff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tariff_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FixedTariff",
                columns: table => new
                {
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TariffId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Charges = table.Column<decimal>(nullable: false),
                    UnitType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FixedTariff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FixedTariff_Tariff_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Meter",
                columns: table => new
                {
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TariffId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    AccountNo = table.Column<string>(nullable: false),
                    ConsumerNo = table.Column<string>(nullable: false),
                    AppUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meter_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Meter_Tariff_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RangedTariff",
                columns: table => new
                {
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TariffId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    UpperRange = table.Column<int>(nullable: false),
                    LowerRange = table.Column<int>(nullable: false),
                    Charges = table.Column<decimal>(nullable: false),
                    UnitType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RangedTariff", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RangedTariff_Tariff_TariffId",
                        column: x => x.TariffId,
                        principalTable: "Tariff",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MeterReading",
                columns: table => new
                {
                    Deleted = table.Column<bool>(nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastModifiedOn = table.Column<DateTime>(nullable: true),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MeterId = table.Column<int>(nullable: false),
                    Reading = table.Column<int>(nullable: false),
                    NotedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeterReading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeterReading_Meter_MeterId",
                        column: x => x.MeterId,
                        principalTable: "Meter",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FixedTariff_TariffId",
                table: "FixedTariff",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_Meter_AppUserId",
                table: "Meter",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Meter_TariffId",
                table: "Meter",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_MeterReading_MeterId",
                table: "MeterReading",
                column: "MeterId");

            migrationBuilder.CreateIndex(
                name: "IX_RangedTariff_TariffId",
                table: "RangedTariff",
                column: "TariffId");

            migrationBuilder.CreateIndex(
                name: "IX_Tariff_AppUserId",
                table: "Tariff",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FixedTariff");

            migrationBuilder.DropTable(
                name: "MeterReading");

            migrationBuilder.DropTable(
                name: "RangedTariff");

            migrationBuilder.DropTable(
                name: "Meter");

            migrationBuilder.DropTable(
                name: "Tariff");

            migrationBuilder.DropTable(
                name: "AppUser");
        }
    }
}
