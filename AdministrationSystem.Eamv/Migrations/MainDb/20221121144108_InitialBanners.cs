using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdministrationSystem.Eamv.Migrations.MainDb
{
    public partial class InitialBanners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    BannerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BannerDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.BannerID);
                    table.ForeignKey(
                        name: "FK_Banners_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Banners_DepartmentID",
                table: "Banners",
                column: "DepartmentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");
        }
    }
}
