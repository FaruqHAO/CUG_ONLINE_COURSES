using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CUG_ONLINE_COURSES.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignedLecturerName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedLecturerName",
                table: "Courses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedLecturerName",
                table: "Courses");
        }
    }
}
