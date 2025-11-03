using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FinalAdjustments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Professors_ProfessorId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Professors_ProfessorId1",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_Secctions_SecctionId",
                table: "Inscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_Students_StudentId",
                table: "Inscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_Students_StudentId1",
                table: "Inscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Secctions_Courses_CourseId",
                table: "Secctions");

            migrationBuilder.DropIndex(
                name: "IX_Secctions_CourseId",
                table: "Secctions");

            migrationBuilder.DropIndex(
                name: "IX_Inscriptions_StudentId1",
                table: "Inscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ProfessorId1",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "StudentId1",
                table: "Inscriptions");

            migrationBuilder.DropColumn(
                name: "ProfessorId1",
                table: "Courses");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Document",
                table: "Students",
                column: "Document",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Secctions_CourseId_Day_StartTime_EndTime",
                table: "Secctions",
                columns: new[] { "CourseId", "Day", "StartTime", "EndTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Professors_Document",
                table: "Professors",
                column: "Document",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Professors_Email",
                table: "Professors",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseName_ProfessorId",
                table: "Courses",
                columns: new[] { "CourseName", "ProfessorId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Professors_ProfessorId",
                table: "Courses",
                column: "ProfessorId",
                principalTable: "Professors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_Secctions_SecctionId",
                table: "Inscriptions",
                column: "SecctionId",
                principalTable: "Secctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_Students_StudentId",
                table: "Inscriptions",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Secctions_Courses_CourseId",
                table: "Secctions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Professors_ProfessorId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_Secctions_SecctionId",
                table: "Inscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Inscriptions_Students_StudentId",
                table: "Inscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Secctions_Courses_CourseId",
                table: "Secctions");

            migrationBuilder.DropIndex(
                name: "IX_Students_Document",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_Email",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Secctions_CourseId_Day_StartTime_EndTime",
                table: "Secctions");

            migrationBuilder.DropIndex(
                name: "IX_Professors_Document",
                table: "Professors");

            migrationBuilder.DropIndex(
                name: "IX_Professors_Email",
                table: "Professors");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CourseName_ProfessorId",
                table: "Courses");

            migrationBuilder.AddColumn<int>(
                name: "StudentId1",
                table: "Inscriptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfessorId1",
                table: "Courses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Secctions_CourseId",
                table: "Secctions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Inscriptions_StudentId1",
                table: "Inscriptions",
                column: "StudentId1");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ProfessorId1",
                table: "Courses",
                column: "ProfessorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Professors_ProfessorId",
                table: "Courses",
                column: "ProfessorId",
                principalTable: "Professors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Professors_ProfessorId1",
                table: "Courses",
                column: "ProfessorId1",
                principalTable: "Professors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_Secctions_SecctionId",
                table: "Inscriptions",
                column: "SecctionId",
                principalTable: "Secctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_Students_StudentId",
                table: "Inscriptions",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inscriptions_Students_StudentId1",
                table: "Inscriptions",
                column: "StudentId1",
                principalTable: "Students",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Secctions_Courses_CourseId",
                table: "Secctions",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
