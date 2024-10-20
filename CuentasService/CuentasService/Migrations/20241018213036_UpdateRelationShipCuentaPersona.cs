using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CuentasService.Migrations
{
    public partial class UpdateRelationShipCuentaPersona : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Personas_ClienteId",
                table: "Cuentas");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Personas_ClienteId",
                table: "Personas",
                column: "ClienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Personas_ClienteId",
                table: "Cuentas",
                column: "ClienteId",
                principalTable: "Personas",
                principalColumn: "ClienteId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cuentas_Personas_ClienteId",
                table: "Cuentas");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Personas_ClienteId",
                table: "Personas");

            migrationBuilder.AddForeignKey(
                name: "FK_Cuentas_Personas_ClienteId",
                table: "Cuentas",
                column: "ClienteId",
                principalTable: "Personas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
