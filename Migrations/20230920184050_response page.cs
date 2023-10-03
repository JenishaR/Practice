using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetEmployeeSurvey.Migrations
{
    public partial class responsepage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "surveyResponseModels",
                columns: table => new
                {
                    SurveyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "surveyResponseModels");
        }
    }
}
