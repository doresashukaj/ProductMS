using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProductMS.Migrations
{
    /// <inheritdoc />
    public partial class UserRolePopulated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "050c25ee-e873-468e-8983-63eaa8325de4", "de3ee07a-f8ff-48c9-a36d-8debdb14abe4" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "050c25ee-e873-468e-8983-63eaa8325de4", "de3ee07a-f8ff-48c9-a36d-8debdb14abe4" });
        }
    }
}
