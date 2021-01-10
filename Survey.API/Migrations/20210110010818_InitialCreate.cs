using Microsoft.EntityFrameworkCore.Migrations;

namespace Survey.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Survey",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Survey", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestion",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuestionType = table.Column<int>(type: "int", nullable: false),
                    SurveyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestion", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SurveyQuestion_Survey_SurveyID",
                        column: x => x.SurveyID,
                        principalTable: "Survey",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResponse",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResponse", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SurveyResponse_Survey_SurveyID",
                        column: x => x.SurveyID,
                        principalTable: "Survey",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyResponse_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuestionAnswer",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsOfferedAnswer = table.Column<bool>(type: "bit", nullable: false),
                    SurveyQuestionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionAnswer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_QuestionAnswer_SurveyQuestion_SurveyQuestionID",
                        column: x => x.SurveyQuestionID,
                        principalTable: "SurveyQuestion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestionResponse",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyResponseID = table.Column<int>(type: "int", nullable: false),
                    SurveyQuestionID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestionResponse", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SurveyQuestionResponse_SurveyQuestion_SurveyQuestionID",
                        column: x => x.SurveyQuestionID,
                        principalTable: "SurveyQuestion",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyQuestionResponse_SurveyResponse_SurveyResponseID",
                        column: x => x.SurveyResponseID,
                        principalTable: "SurveyResponse",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SurveyQuestionAnswerResponse",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SurveyQuestionResponseID = table.Column<int>(type: "int", nullable: false),
                    QuestionAnswerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyQuestionAnswerResponse", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SurveyQuestionAnswerResponse_QuestionAnswer_QuestionAnswerID",
                        column: x => x.QuestionAnswerID,
                        principalTable: "QuestionAnswer",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SurveyQuestionAnswerResponse_SurveyQuestionResponse_SurveyQuestionResponseID",
                        column: x => x.SurveyQuestionResponseID,
                        principalTable: "SurveyQuestionResponse",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuestionAnswer_SurveyQuestionID",
                table: "QuestionAnswer",
                column: "SurveyQuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestion_SurveyID",
                table: "SurveyQuestion",
                column: "SurveyID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionAnswerResponse_QuestionAnswerID",
                table: "SurveyQuestionAnswerResponse",
                column: "QuestionAnswerID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionAnswerResponse_SurveyQuestionResponseID",
                table: "SurveyQuestionAnswerResponse",
                column: "SurveyQuestionResponseID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionResponse_SurveyQuestionID",
                table: "SurveyQuestionResponse",
                column: "SurveyQuestionID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyQuestionResponse_SurveyResponseID",
                table: "SurveyQuestionResponse",
                column: "SurveyResponseID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponse_SurveyID",
                table: "SurveyResponse",
                column: "SurveyID");

            migrationBuilder.CreateIndex(
                name: "IX_SurveyResponse_UserID",
                table: "SurveyResponse",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SurveyQuestionAnswerResponse");

            migrationBuilder.DropTable(
                name: "QuestionAnswer");

            migrationBuilder.DropTable(
                name: "SurveyQuestionResponse");

            migrationBuilder.DropTable(
                name: "SurveyQuestion");

            migrationBuilder.DropTable(
                name: "SurveyResponse");

            migrationBuilder.DropTable(
                name: "Survey");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
