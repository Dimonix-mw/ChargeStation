using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PaymentService.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Filials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Filials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PumpModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    is_quick_charge = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PumpModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FilialId = table.Column<int>(type: "integer", nullable: false),
                    PumpModelId = table.Column<int>(type: "integer", nullable: false),
                    Cost = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prices_Filials_FilialId",
                        column: x => x.FilialId,
                        principalTable: "Filials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prices_PumpModels_PumpModelId",
                        column: x => x.PumpModelId,
                        principalTable: "PumpModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pumps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    ModelId = table.Column<int>(type: "integer", nullable: false),
                    FilialId = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pumps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pumps_Filials_FilialId",
                        column: x => x.FilialId,
                        principalTable: "Filials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pumps_PumpModels_ModelId",
                        column: x => x.ModelId,
                        principalTable: "PumpModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fillings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PumpId = table.Column<int>(type: "integer", nullable: false),
                    PromotionId = table.Column<int>(type: "integer", nullable: false),
                    PromotionAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalMoneyAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    BonusAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    BonusCalculateRuleId = table.Column<int>(type: "integer", nullable: false),
                    Minutes = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fillings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fillings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Minutes = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    FillingId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Fillings_FillingId",
                        column: x => x.FillingId,
                        principalTable: "Fillings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fillings_Id",
                table: "Fillings",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Fillings_UserId",
                table: "Fillings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_FilialId",
                table: "Prices",
                column: "FilialId");

            migrationBuilder.CreateIndex(
                name: "IX_Prices_Id",
                table: "Prices",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_PumpModelId",
                table: "Prices",
                column: "PumpModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Pumps_FilialId",
                table: "Pumps",
                column: "FilialId");

            migrationBuilder.CreateIndex(
                name: "IX_Pumps_Id",
                table: "Pumps",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pumps_ModelId",
                table: "Pumps",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_FillingId",
                table: "Sessions",
                column: "FillingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_Id",
                table: "Sessions",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_UserId",
                table: "Sessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Pumps");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Filials");

            migrationBuilder.DropTable(
                name: "PumpModels");

            migrationBuilder.DropTable(
                name: "Fillings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
