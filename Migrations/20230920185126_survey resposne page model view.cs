using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetEmployeeSurvey.Migrations
{
    public partial class surveyresposnepagemodelview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "surveyResponseModels");

            migrationBuilder.CreateTable(
                name: "surveyResponsePageModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_surveyResponsePageModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_surveyResponsePageModels_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionResponse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsYesOrNoQuestion = table.Column<bool>(type: "bit", nullable: false),
                    YesOrNoResponse = table.Column<bool>(type: "bit", nullable: false),
                    TextResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SurveyResponsePageModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionResponse_surveyResponsePageModels_SurveyResponsePageModelId",
                        column: x => x.SurveyResponsePageModelId,
                        principalTable: "surveyResponsePageModels",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionResponse_SurveyResponsePageModelId",
                table: "QuestionResponse",
                column: "SurveyResponsePageModelId");

            migrationBuilder.CreateIndex(
                name: "IX_surveyResponsePageModels_SurveyId",
                table: "surveyResponsePageModels",
                column: "SurveyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuestionResponse");

            migrationBuilder.DropTable(
                name: "surveyResponsePageModels");

            migrationBuilder.CreateTable(
                name: "surveyResponseModels",
                columns: table => new
                {
                    SurveyId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_surveyResponseModels_Surveys_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "Surveys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_surveyResponseModels_SurveyId",
                table: "surveyResponseModels",
                column: "SurveyId");
        }
    }
}
