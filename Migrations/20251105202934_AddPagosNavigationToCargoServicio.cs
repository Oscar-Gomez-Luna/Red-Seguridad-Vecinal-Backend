using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_RSV.Migrations
{
    /// <inheritdoc />
    public partial class AddPagosNavigationToCargoServicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CargoServicioID",
                table: "Pagos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CargoServicioID",
                table: "Pagos",
                column: "CargoServicioID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_CargosServicios_CargoServicioID",
                table: "Pagos",
                column: "CargoServicioID",
                principalTable: "CargosServicios",
                principalColumn: "CargoServicioID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_CargosServicios_CargoServicioID",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_CargoServicioID",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "CargoServicioID",
                table: "Pagos");
        }
    }
}
