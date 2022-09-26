using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChargeService.DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "FillingNumbers");

            migrationBuilder.CreateSequence<int>(
                name: "SessionNumbers");

            migrationBuilder.CreateTable(
                name: "Fillings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"FillingNumbers\"')"),
                    PumpId = table.Column<int>(type: "integer", nullable: false),
                    PromotionId = table.Column<int>(type: "integer", nullable: false),
                    PromotionAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalMoneyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    BonusAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    BonusCalculateRuleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fillings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false, defaultValueSql: "nextval('\"SessionNumbers\"')"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Minutes = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FillingId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Fillings_Id",
                        column: x => x.Id,
                        principalTable: "Fillings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Fillings");

            migrationBuilder.DropSequence(
                name: "FillingNumbers");

            migrationBuilder.DropSequence(
                name: "SessionNumbers");
        }
    }
}
