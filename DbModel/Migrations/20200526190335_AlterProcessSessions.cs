using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCrawler.WebApp.DbModel.Migrations
{
    public partial class AlterProcessSessions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessSessions_AspNetUsers_CreatorId",
                table: "ProcessSessions");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "ProcessSessions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AppName",
                table: "ProcessSessions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessSessions_AspNetUsers_CreatorId",
                table: "ProcessSessions",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessSessions_State",
                table: "ProcessSessions",
                column: "State");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessSessions_AspNetUsers_CreatorId",
                table: "ProcessSessions");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "ProcessSessions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "AppName",
                table: "ProcessSessions",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessSessions_AspNetUsers_CreatorId",
                table: "ProcessSessions",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropIndex(
                name: "IX_ProcessSessions_State",
                table: "ProcessSessions");
        }
    }
}
