using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IncludeTypeBackend.Migrations
{
    /// <inheritdoc />
    public partial class includetype_initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Privacy",
                schema: "public",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: false),
                    Picture = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Contact = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Education = table.Column<string>(type: "text", nullable: false),
                    Companies = table.Column<string>(type: "text", nullable: false),
                    Skills = table.Column<string>(type: "text", nullable: false),
                    Experience = table.Column<string>(type: "text", nullable: false),
                    Projects = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privacy", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ProfessionalProfile",
                schema: "public",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Education = table.Column<string>(type: "text", nullable: false),
                    Companies = table.Column<string>(type: "text", nullable: false),
                    Skills = table.Column<string>(type: "text", nullable: false),
                    ExperienceYears = table.Column<int>(type: "integer", nullable: false),
                    ExperienceMonths = table.Column<int>(type: "integer", nullable: false),
                    Projects = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfessionalProfile", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    About = table.Column<string>(type: "text", nullable: false),
                    Documentation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectIssue",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ProjId = table.Column<string>(type: "text", nullable: false),
                    ProjName = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<string>(type: "text", nullable: false),
                    Details = table.Column<string>(type: "text", nullable: false),
                    Deadline = table.Column<string>(type: "text", nullable: false),
                    Assigned = table.Column<string>(type: "text", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    Priority = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectIssue", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMember",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ProjName = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMember", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectTask",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    ProjId = table.Column<string>(type: "text", nullable: false),
                    ProjName = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Date = table.Column<string>(type: "text", nullable: false),
                    Details = table.Column<string>(type: "text", nullable: false),
                    Deadline = table.Column<string>(type: "text", nullable: false),
                    Assigned = table.Column<string>(type: "text", nullable: false),
                    Completed = table.Column<bool>(type: "boolean", nullable: false),
                    Priority = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectTask", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Pincode = table.Column<string>(type: "text", nullable: false),
                    Contact = table.Column<string>(type: "text", nullable: false),
                    Picture = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserVerification",
                schema: "public",
                columns: table => new
                {
                    UniqueString = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreationTime = table.Column<string>(type: "text", nullable: false),
                    ExpirationTime = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVerification", x => x.UniqueString);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Privacy",
                columns: new[] { "UserId", "Address", "Bio", "Companies", "Contact", "Education", "Email", "Experience", "Name", "Picture", "Projects", "Skills" },
                values: new object[] { "1", "Public", "Public", "Public", "Public", "Public", "Public", "Public", "Public", "Public", "Public", "Public" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "ProfessionalProfile",
                columns: new[] { "UserId", "Companies", "Education", "ExperienceMonths", "ExperienceYears", "Projects", "Skills" },
                values: new object[] { "1", "", "", 0, 0, "", "" });

            migrationBuilder.InsertData(
                schema: "public",
                table: "User",
                columns: new[] { "Id", "Address", "Bio", "City", "Contact", "Country", "Email", "FirstName", "IsAdmin", "LastName", "Password", "Picture", "Pincode", "State", "Username" },
                values: new object[] { "1", "", "", "", "", "", "subhamkarmakar0901@gmail.com", "Subham", true, "Karmakar", "$2a$11$8rAS9ZcDyO7BLx0qJq18peC3ThxtNMuJRfNncJQ28.Nc4N2dWCUmG", "", "", "", "SubhamK108" });

            migrationBuilder.CreateIndex(
                name: "IX_Project_Name",
                schema: "public",
                table: "Project",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "public",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                schema: "public",
                table: "User",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Privacy",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProfessionalProfile",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Project",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProjectIssue",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProjectMember",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProjectTask",
                schema: "public");

            migrationBuilder.DropTable(
                name: "User",
                schema: "public");

            migrationBuilder.DropTable(
                name: "UserVerification",
                schema: "public");
        }
    }
}
