using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorMovies.Server.Migrations
{
    public partial class AdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'4748b98a-d35e-449e-b7fa-013ff27247ca', N'Admin', N'ADMIN', NULL)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
