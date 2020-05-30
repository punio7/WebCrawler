using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCrawler.WebApp.DbModel.Migrations
{
    public partial class AddIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WorkerName",
                table: "WorkerConnections",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkerConnections_WorkerName",
                table: "WorkerConnections",
                column: "WorkerName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WorkerConnections_WorkerName",
                table: "WorkerConnections");

            migrationBuilder.AlterColumn<string>(
                name: "WorkerName",
                table: "WorkerConnections",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);
        }
    }
}
