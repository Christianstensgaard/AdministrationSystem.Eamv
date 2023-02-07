using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdministrationSystem.Eamv.Migrations.MainDb
{
    public partial class InitialRooms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rooms_DepartmentID",
                table: "Rooms",
                column: "DepartmentID");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_Departments_DepartmentID",
                table: "Rooms",
                column: "DepartmentID",
                principalTable: "Departments",
                principalColumn: "DepartmentID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_Departments_DepartmentID",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_DepartmentID",
                table: "Rooms");
        }
    }
}
