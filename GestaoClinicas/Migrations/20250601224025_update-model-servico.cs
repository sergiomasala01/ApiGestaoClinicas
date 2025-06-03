using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiGestaoClinicas.Migrations
{
    /// <inheritdoc />
    public partial class updatemodelservico : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Situacao",
                table: "Atendimentos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Situacao",
                table: "Atendimentos");
        }
    }
}
