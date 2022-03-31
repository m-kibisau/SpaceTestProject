using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SpaceTestProject.Api.Migrations.ApplicationDbMigrations
{
    public partial class InitDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WatchListItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TitleId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsWatched = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchListItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WatchListEmailLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WatchListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SendingTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchListEmailLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchListEmailLogs_WatchListItems_WatchListId",
                        column: x => x.WatchListId,
                        principalTable: "WatchListItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WatchListEmailLogs_WatchListId",
                table: "WatchListEmailLogs",
                column: "WatchListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WatchListEmailLogs");

            migrationBuilder.DropTable(
                name: "WatchListItems");
        }
    }
}
