using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestCore_Api.Migrations
{
    /// <inheritdoc />
    public partial class v5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "st_role",
                columns: table => new
                {
                    IdRole = table.Column<string>(type: "text", nullable: false),
                    NameRole = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_st_role", x => x.IdRole);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "st_role");
        }
    }
}
