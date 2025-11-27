using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_RSV.Migrations
{
    /// <inheritdoc />
    public partial class CambiosDeRelacionesPagoADetallePago : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_CargosMantenimiento_CargoMantenimientoID",
                table: "Pagos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagos_CargosServicios_CargoServicioID",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_CargoMantenimientoID",
                table: "Pagos");

            migrationBuilder.DropIndex(
                name: "IX_Pagos_CargoServicioID",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "CargoMantenimientoID",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "CargoServicioID",
                table: "Pagos");

            migrationBuilder.DropColumn(
                name: "CargoID",
                table: "DetallePago");

            migrationBuilder.DropColumn(
                name: "TipoCargo",
                table: "DetallePago");

            migrationBuilder.AddColumn<int>(
                name: "CargoMantenimientoID",
                table: "DetallePago",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CargoServicioID",
                table: "DetallePago",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DetallePago_CargoMantenimientoID",
                table: "DetallePago",
                column: "CargoMantenimientoID");

            migrationBuilder.CreateIndex(
                name: "IX_DetallePago_CargoServicioID",
                table: "DetallePago",
                column: "CargoServicioID");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallePago_CargosMantenimiento_CargoMantenimientoID",
                table: "DetallePago",
                column: "CargoMantenimientoID",
                principalTable: "CargosMantenimiento",
                principalColumn: "CargoMantenimientoID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallePago_CargosServicios_CargoServicioID",
                table: "DetallePago",
                column: "CargoServicioID",
                principalTable: "CargosServicios",
                principalColumn: "CargoServicioID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallePago_CargosMantenimiento_CargoMantenimientoID",
                table: "DetallePago");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallePago_CargosServicios_CargoServicioID",
                table: "DetallePago");

            migrationBuilder.DropIndex(
                name: "IX_DetallePago_CargoMantenimientoID",
                table: "DetallePago");

            migrationBuilder.DropIndex(
                name: "IX_DetallePago_CargoServicioID",
                table: "DetallePago");

            migrationBuilder.DropColumn(
                name: "CargoMantenimientoID",
                table: "DetallePago");

            migrationBuilder.DropColumn(
                name: "CargoServicioID",
                table: "DetallePago");

            migrationBuilder.AddColumn<int>(
                name: "CargoMantenimientoID",
                table: "Pagos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CargoServicioID",
                table: "Pagos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CargoID",
                table: "DetallePago",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "TipoCargo",
                table: "DetallePago",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CargoMantenimientoID",
                table: "Pagos",
                column: "CargoMantenimientoID");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_CargoServicioID",
                table: "Pagos",
                column: "CargoServicioID");

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_CargosMantenimiento_CargoMantenimientoID",
                table: "Pagos",
                column: "CargoMantenimientoID",
                principalTable: "CargosMantenimiento",
                principalColumn: "CargoMantenimientoID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagos_CargosServicios_CargoServicioID",
                table: "Pagos",
                column: "CargoServicioID",
                principalTable: "CargosServicios",
                principalColumn: "CargoServicioID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
