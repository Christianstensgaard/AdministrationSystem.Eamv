using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdministrationSystem.Eamv.Migrations.FeedbackDb
{
    public partial class Initialv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FeedbackName",
                table: "Feedbacks",
                newName: "FeedbackDisc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FeedbackDisc",
                table: "Feedbacks",
                newName: "FeedbackName");
        }
    }
}
