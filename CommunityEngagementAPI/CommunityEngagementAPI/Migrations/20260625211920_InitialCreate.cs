using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommunityEngagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "campus",
                columns: table => new
                {
                    campus_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_campus", x => x.campus_id);
                });

            migrationBuilder.CreateTable(
                name: "faculty",
                columns: table => new
                {
                    faculty_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_faculty", x => x.faculty_id);
                });

            migrationBuilder.CreateTable(
                name: "sdg",
                columns: table => new
                {
                    sdg_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    icon = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sdg", x => x.sdg_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    id_number = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "varchar(20)", nullable: false),
                    is_suspended = table.Column<bool>(type: "boolean", nullable: false),
                    date_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    campus_id = table.Column<int>(type: "integer", nullable: true),
                    faculty_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_user_campus_campus_id",
                        column: x => x.campus_id,
                        principalTable: "campus",
                        principalColumn: "campus_id");
                    table.ForeignKey(
                        name: "fk_user_faculty_faculty_id",
                        column: x => x.faculty_id,
                        principalTable: "faculty",
                        principalColumn: "faculty_id");
                });

            migrationBuilder.CreateTable(
                name: "ngo_application",
                columns: table => new
                {
                    application_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    reg_number = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    close_campus = table.Column<string>(type: "text", nullable: false),
                    address = table.Column<string>(type: "text", nullable: false),
                    rep_name = table.Column<string>(type: "text", nullable: false),
                    rep_position = table.Column<string>(type: "text", nullable: false),
                    rep_phone = table.Column<string>(type: "text", nullable: false),
                    key_focus = table.Column<string>(type: "text", nullable: false),
                    obligations = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    website = table.Column<string>(type: "text", nullable: true),
                    is_viewed = table.Column<bool>(type: "boolean", nullable: false),
                    date_submitted = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    specialist_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ngo_application", x => x.application_id);
                    table.ForeignKey(
                        name: "fk_ngo_application_user_specialist_id",
                        column: x => x.specialist_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_demographics",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    geo_area = table.Column<string>(type: "text", nullable: true),
                    home_language = table.Column<string>(type: "text", nullable: true),
                    gender = table.Column<int>(type: "integer", nullable: true),
                    is_international = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_demographics", x => x.user_id);
                    table.ForeignKey(
                        name: "fk_user_demographics_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "volunteer",
                columns: table => new
                {
                    volunteer_id = table.Column<int>(type: "integer", nullable: false),
                    volunteer_type = table.Column<int>(type: "integer", nullable: true),
                    is_completed = table.Column<bool>(type: "boolean", nullable: false),
                    recruiter_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteer", x => x.volunteer_id);
                    table.ForeignKey(
                        name: "fk_volunteer_user_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "alumni",
                columns: table => new
                {
                    alumni_id = table.Column<int>(type: "integer", nullable: false),
                    year_graduated = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_alumni", x => x.alumni_id);
                    table.ForeignKey(
                        name: "fk_alumni_volunteer_alumni_id",
                        column: x => x.alumni_id,
                        principalTable: "volunteer",
                        principalColumn: "volunteer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "staff",
                columns: table => new
                {
                    staff_id = table.Column<int>(type: "integer", nullable: false),
                    staff_number = table.Column<string>(type: "text", nullable: true),
                    department = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_staff", x => x.staff_id);
                    table.ForeignKey(
                        name: "fk_staff_volunteer_staff_id",
                        column: x => x.staff_id,
                        principalTable: "volunteer",
                        principalColumn: "volunteer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student",
                columns: table => new
                {
                    student_id = table.Column<int>(type: "integer", nullable: false),
                    student_number = table.Column<string>(type: "text", nullable: true),
                    year_of_study = table.Column<int>(type: "integer", nullable: true),
                    qualification = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student", x => x.student_id);
                    table.ForeignKey(
                        name: "fk_student_volunteer_student_id",
                        column: x => x.student_id,
                        principalTable: "volunteer",
                        principalColumn: "volunteer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "volunteer_sdg",
                columns: table => new
                {
                    volunteer_id = table.Column<int>(type: "integer", nullable: false),
                    sdg_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_volunteer_sdg", x => new { x.volunteer_id, x.sdg_id });
                    table.ForeignKey(
                        name: "fk_volunteer_sdg_sdg_sdg_id",
                        column: x => x.sdg_id,
                        principalTable: "sdg",
                        principalColumn: "sdg_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_volunteer_sdg_volunteer_volunteer_id",
                        column: x => x.volunteer_id,
                        principalTable: "volunteer",
                        principalColumn: "volunteer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_ngo_application_specialist_id",
                table: "ngo_application",
                column: "specialist_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_campus_id",
                table: "user",
                column: "campus_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_faculty_id",
                table: "user",
                column: "faculty_id");

            migrationBuilder.CreateIndex(
                name: "ix_volunteer_sdg_sdg_id",
                table: "volunteer_sdg",
                column: "sdg_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "alumni");

            migrationBuilder.DropTable(
                name: "ngo_application");

            migrationBuilder.DropTable(
                name: "staff");

            migrationBuilder.DropTable(
                name: "student");

            migrationBuilder.DropTable(
                name: "user_demographics");

            migrationBuilder.DropTable(
                name: "volunteer_sdg");

            migrationBuilder.DropTable(
                name: "sdg");

            migrationBuilder.DropTable(
                name: "volunteer");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "campus");

            migrationBuilder.DropTable(
                name: "faculty");
        }
    }
}
