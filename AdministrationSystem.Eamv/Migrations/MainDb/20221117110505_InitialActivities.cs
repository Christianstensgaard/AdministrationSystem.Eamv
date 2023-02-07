using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdministrationSystem.Eamv.Migrations.MainDb
{
    public partial class InitialActivities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activities",
                columns: table => new
                {
                    Activityid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ByWhom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentID = table.Column<int>(type: "int", nullable: false),
                    RoomID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activities", x => x.Activityid);
                    table.ForeignKey(
                        name: "FK_Activities_Departments_DepartmentID",
                        column: x => x.DepartmentID,
                        principalTable: "Departments",
                        principalColumn: "DepartmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activities_Rooms_RoomID",
                        column: x => x.RoomID,
                        principalTable: "Rooms",
                        principalColumn: "RoomID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Date",
                columns: table => new
                {
                    DateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Activityid = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Date", x => x.DateID);
                    table.ForeignKey(
                        name: "FK_Date_Activities_Activityid",
                        column: x => x.Activityid,
                        principalTable: "Activities",
                        principalColumn: "Activityid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activities_DepartmentID",
                table: "Activities",
                column: "DepartmentID");

            migrationBuilder.CreateIndex(
                name: "IX_Activities_RoomID",
                table: "Activities",
                column: "RoomID");

            migrationBuilder.CreateIndex(
                name: "IX_Date_Activityid",
                table: "Date",
                column: "Activityid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Date");

            migrationBuilder.DropTable(
                name: "Activities");
        }
    }
}
