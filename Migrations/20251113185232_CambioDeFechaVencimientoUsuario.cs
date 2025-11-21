using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_RSV.Migrations
{
    /// <inheritdoc />
    public partial class CambioDeFechaVencimientoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FechaVencimiento",
                table: "CuentaUsuario",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "FechaVencimiento",
                table: "CuentaUsuario",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
