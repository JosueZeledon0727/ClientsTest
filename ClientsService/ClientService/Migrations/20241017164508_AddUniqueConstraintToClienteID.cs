using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientService.Migrations
{
    public partial class AddUniqueConstraintToClienteID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Personas_ClienteId",
                table: "Personas",
                column: "ClienteId",
                unique: true,
                filter: "[ClienteId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Personas_ClienteId",
                table: "Personas");
        }
    }
}
