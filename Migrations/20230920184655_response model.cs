using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetEmployeeSurvey.Migrations
{
    public partial class responsemodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "surveyResponseModels",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_surveyResponseModels_SurveyId",
                table: "surveyResponseModels",
                column: "SurveyId");

            migrationBuilder.AddForeignKey(
                name: "FK_surveyResponseModels_Surveys_SurveyId",
                table: "surveyResponseModels",
                column: "SurveyId",
                principalTable: "Surveys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_surveyResponseModels_Surveys_SurveyId",
                table: "surveyResponseModels");

            migrationBuilder.DropIndex(
                name: "IX_surveyResponseModels_SurveyId",
                table: "surveyResponseModels");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "surveyResponseModels");
        }
    }
}
