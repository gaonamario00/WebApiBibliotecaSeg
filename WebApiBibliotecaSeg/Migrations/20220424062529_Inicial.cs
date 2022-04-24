using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiBibliotecaSeg.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "autores",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_autores", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "libros",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    titulo = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libros", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "editorial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    autorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_editorial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_editorial_autores_autorId",
                        column: x => x.autorId,
                        principalTable: "autores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "libroAutor",
                columns: table => new
                {
                    libroId = table.Column<int>(type: "int", nullable: false),
                    autorId = table.Column<int>(type: "int", nullable: false),
                    orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_libroAutor", x => new { x.libroId, x.autorId });
                    table.ForeignKey(
                        name: "FK_libroAutor_autores_autorId",
                        column: x => x.autorId,
                        principalTable: "autores",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_libroAutor_libros_libroId",
                        column: x => x.libroId,
                        principalTable: "libros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_editorial_autorId",
                table: "editorial",
                column: "autorId");

            migrationBuilder.CreateIndex(
                name: "IX_libroAutor_autorId",
                table: "libroAutor",
                column: "autorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "editorial");

            migrationBuilder.DropTable(
                name: "libroAutor");

            migrationBuilder.DropTable(
                name: "autores");

            migrationBuilder.DropTable(
                name: "libros");
        }
    }
}
