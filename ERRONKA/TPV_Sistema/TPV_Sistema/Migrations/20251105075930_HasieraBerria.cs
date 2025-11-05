using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TPV_Sistema.Migrations
{
    /// <inheritdoc />
    public partial class HasieraBerria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Erabiltzaileak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ErabiltzaileIzena = table.Column<string>(type: "TEXT", nullable: false),
                    Pasahitza = table.Column<string>(type: "TEXT", nullable: false),
                    Rola = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Erabiltzaileak", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Mahaiak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MahaiIzena = table.Column<string>(type: "TEXT", nullable: false),
                    Edukiera = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mahaiak", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Produktuak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Izena = table.Column<string>(type: "TEXT", nullable: false),
                    Prezioa = table.Column<double>(type: "REAL", nullable: false),
                    Stocka = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produktuak", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Erreserbak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Otordua = table.Column<string>(type: "TEXT", nullable: false),
                    ErabiltzaileId = table.Column<int>(type: "INTEGER", nullable: false),
                    MahaiaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ErabiltzaileaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Erreserbak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Erreserbak_Erabiltzaileak_ErabiltzaileaId",
                        column: x => x.ErabiltzaileaId,
                        principalTable: "Erabiltzaileak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Erreserbak_Mahaiak_MahaiaId",
                        column: x => x.MahaiaId,
                        principalTable: "Mahaiak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Eskaerak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Data = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Guztira = table.Column<double>(type: "REAL", nullable: false),
                    ErabiltzaileId = table.Column<int>(type: "INTEGER", nullable: false),
                    ErabiltzaileaId = table.Column<int>(type: "INTEGER", nullable: false),
                    EskaeraId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProduktuaId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eskaerak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eskaerak_Erabiltzaileak_ErabiltzaileaId",
                        column: x => x.ErabiltzaileaId,
                        principalTable: "Erabiltzaileak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Eskaerak_Eskaerak_EskaeraId",
                        column: x => x.EskaeraId,
                        principalTable: "Eskaerak",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Eskaerak_Produktuak_ProduktuaId",
                        column: x => x.ProduktuaId,
                        principalTable: "Produktuak",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EskaeraLerroak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Kantitatea = table.Column<int>(type: "INTEGER", nullable: false),
                    PrezioaUnitateko = table.Column<double>(type: "REAL", nullable: false),
                    EskaeraId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProduktuaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EskaeraLerroak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EskaeraLerroak_Eskaerak_EskaeraId",
                        column: x => x.EskaeraId,
                        principalTable: "Eskaerak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EskaeraLerroak_Produktuak_ProduktuaId",
                        column: x => x.ProduktuaId,
                        principalTable: "Produktuak",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Erreserbak_ErabiltzaileaId",
                table: "Erreserbak",
                column: "ErabiltzaileaId");

            migrationBuilder.CreateIndex(
                name: "IX_Erreserbak_MahaiaId",
                table: "Erreserbak",
                column: "MahaiaId");

            migrationBuilder.CreateIndex(
                name: "IX_Eskaerak_ErabiltzaileaId",
                table: "Eskaerak",
                column: "ErabiltzaileaId");

            migrationBuilder.CreateIndex(
                name: "IX_Eskaerak_EskaeraId",
                table: "Eskaerak",
                column: "EskaeraId");

            migrationBuilder.CreateIndex(
                name: "IX_Eskaerak_ProduktuaId",
                table: "Eskaerak",
                column: "ProduktuaId");

            migrationBuilder.CreateIndex(
                name: "IX_EskaeraLerroak_EskaeraId",
                table: "EskaeraLerroak",
                column: "EskaeraId");

            migrationBuilder.CreateIndex(
                name: "IX_EskaeraLerroak_ProduktuaId",
                table: "EskaeraLerroak",
                column: "ProduktuaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Erreserbak");

            migrationBuilder.DropTable(
                name: "EskaeraLerroak");

            migrationBuilder.DropTable(
                name: "Mahaiak");

            migrationBuilder.DropTable(
                name: "Eskaerak");

            migrationBuilder.DropTable(
                name: "Erabiltzaileak");

            migrationBuilder.DropTable(
                name: "Produktuak");
        }
    }
}
