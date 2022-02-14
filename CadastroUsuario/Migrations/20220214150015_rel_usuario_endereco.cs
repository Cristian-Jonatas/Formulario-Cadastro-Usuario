using Microsoft.EntityFrameworkCore.Migrations;

namespace CadastroUsuario.Migrations
{
    public partial class rel_usuario_endereco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdUsuario",
                table: "Endereco",
                newName: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Endereco_UsuarioId",
                table: "Endereco",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Endereco_Usuario_UsuarioId",
                table: "Endereco",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Endereco_Usuario_UsuarioId",
                table: "Endereco");

            migrationBuilder.DropIndex(
                name: "IX_Endereco_UsuarioId",
                table: "Endereco");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Endereco",
                newName: "IdUsuario");
        }
    }
}
