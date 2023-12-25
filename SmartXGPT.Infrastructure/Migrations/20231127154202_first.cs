using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartXGPT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnswerString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Step = table.Column<int>(type: "int", nullable: true),
                    Length = table.Column<int>(type: "int", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SurveyResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RestaurantName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    FoodQuality = table.Column<int>(type: "int", nullable: false),
                    Cleanliness = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Behavior = table.Column<int>(type: "int", nullable: false),
                    Overall = table.Column<int>(type: "int", nullable: false),
                    DoTranslate = table.Column<bool>(type: "bit", nullable: false),
                    DoSimple = table.Column<bool>(type: "bit", nullable: false),
                    DoHierarchy = table.Column<bool>(type: "bit", nullable: false),
                    DoSms = table.Column<bool>(type: "bit", nullable: false),
                    DoCommentSms = table.Column<bool>(type: "bit", nullable: false),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SurveyResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageGPTs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SurveyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AskThis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RawAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnQuestion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EnAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuccess = table.Column<bool>(type: "bit", nullable: false),
                    IsAsked = table.Column<bool>(type: "bit", nullable: false),
                    DoTranslate = table.Column<bool>(type: "bit", nullable: false),
                    IsManual = table.Column<bool>(type: "bit", nullable: false),
                    IsManualWithContext = table.Column<bool>(type: "bit", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Step = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UsedTokens = table.Column<int>(type: "int", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageGPTs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageGPTs_SurveyResults_SurveyId",
                        column: x => x.SurveyId,
                        principalTable: "SurveyResults",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestinString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Step = table.Column<int>(type: "int", nullable: true),
                    Length = table.Column<int>(type: "int", nullable: true),
                    AnswerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    messageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsertTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRemoved = table.Column<bool>(type: "bit", nullable: false),
                    RemoveTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Answers_AnswerId",
                        column: x => x.AnswerId,
                        principalTable: "Answers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Questions_MessageGPTs_messageId",
                        column: x => x.messageId,
                        principalTable: "MessageGPTs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageGPTs_SurveyId",
                table: "MessageGPTs",
                column: "SurveyId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_AnswerId",
                table: "Questions",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_messageId",
                table: "Questions",
                column: "messageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "MessageGPTs");

            migrationBuilder.DropTable(
                name: "SurveyResults");
        }
    }
}
