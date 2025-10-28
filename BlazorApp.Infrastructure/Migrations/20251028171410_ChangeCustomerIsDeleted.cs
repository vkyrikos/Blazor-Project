using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCustomerIsDeleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDelete",
                table: "Customers",
                newName: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Customers",
                newName: "IsDelete");
        }
    }
}
