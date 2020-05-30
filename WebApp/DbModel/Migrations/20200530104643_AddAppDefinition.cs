using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebCrawler.WebApp.DbModel.Migrations
{
    public partial class AddAppDefinition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppName",
                table: "ProcessSessions");

            migrationBuilder.AddColumn<long>(
                name: "AppId",
                table: "ProcessSessions",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "AppDefinitions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDefinitions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessSessions_AppId",
                table: "ProcessSessions",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_AppDefinitions_Name",
                table: "AppDefinitions",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessSessions_AppDefinitions_AppId",
                table: "ProcessSessions",
                column: "AppId",
                principalTable: "AppDefinitions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessSessions_AppDefinitions_AppId",
                table: "ProcessSessions");

            migrationBuilder.DropTable(
                name: "AppDefinitions");

            migrationBuilder.DropIndex(
                name: "IX_ProcessSessions_AppId",
                table: "ProcessSessions");

            migrationBuilder.DropColumn(
                name: "AppId",
                table: "ProcessSessions");

            migrationBuilder.AddColumn<string>(
                name: "AppName",
                table: "ProcessSessions",
                nullable: false,
                defaultValue: "");
        }
    }
}
