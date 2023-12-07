using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobantChallenge.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDateTimeColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    department = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__departme__3213E83FBD7ECA4F", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    job = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__jobs__3213E83F96F7BB39", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "hired_employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    datetime = table.Column<DateTime>(type: "datetime2", unicode: false, maxLength: 255, nullable: true),
                    department_id = table.Column<int>(type: "int", nullable: true),
                    job_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__hired_em__3213E83F58807ED0", x => x.id);
                    table.ForeignKey(
                        name: "FK__hired_emp__depar__286302EC",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__hired_emp__job_i__29572725",
                        column: x => x.job_id,
                        principalTable: "jobs",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_hired_employees_department_id",
                table: "hired_employees",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_hired_employees_job_id",
                table: "hired_employees",
                column: "job_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "hired_employees");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "jobs");
        }
    }
}
