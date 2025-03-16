using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialNetwork.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class dbset4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Relationships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserEntityId",
                table: "Relationships",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Avatar",
                table: "GroupChats",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Relationships_UserEntityId",
                table: "Relationships",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Relationships_Users_UserEntityId",
                table: "Relationships",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Relationships_Users_UserEntityId",
                table: "Relationships");

            migrationBuilder.DropIndex(
                name: "IX_Relationships_UserEntityId",
                table: "Relationships");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Relationships");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "Relationships");

            migrationBuilder.DropColumn(
                name: "Avatar",
                table: "GroupChats");
        }
    }
}
