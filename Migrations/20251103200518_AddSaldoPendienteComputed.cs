using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_RSV.Migrations
{
    /// <inheritdoc />
    public partial class AddSaldoPendienteComputed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SaldoPendiente",
                table: "CargosServicios",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                computedColumnSql: "[Monto] - [MontoPagado]",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AlterColumn<decimal>(
                name: "SaldoPendiente",
                table: "CargosMantenimiento",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                computedColumnSql: "[Monto] - [MontoPagado]",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "SaldoPendiente",
                table: "CargosServicios",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldComputedColumnSql: "[Monto] - [MontoPagado]");

            migrationBuilder.AlterColumn<decimal>(
                name: "SaldoPendiente",
                table: "CargosMantenimiento",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldComputedColumnSql: "[Monto] - [MontoPagado]");
        }
    }
}
