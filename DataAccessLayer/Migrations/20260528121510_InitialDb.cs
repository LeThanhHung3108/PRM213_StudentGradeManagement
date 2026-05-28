using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "subject_classes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubjectCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SubjectName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    ClassName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Semester = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_subject_classes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "score_records",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SubjectClassId = table.Column<int>(type: "integer", nullable: false),
                    RollNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FinalGrade = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Status = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false, defaultValue: "Fail"),
                    ImportedDate = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_score_records", x => x.Id);
                    table.CheckConstraint("CK_score_records_status_allowed", "\"Status\" IN ('Fail', 'Pass')");
                    table.ForeignKey(
                        name: "FK_score_records_subject_classes_SubjectClassId",
                        column: x => x.SubjectClassId,
                        principalTable: "subject_classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "score_components",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScoreRecordId = table.Column<int>(type: "integer", nullable: false),
                    ComponentName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ScoreValue = table.Column<double>(type: "double precision", nullable: false, defaultValue: 0.0),
                    Weight = table.Column<double>(type: "double precision", nullable: false, defaultValue: 1.0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_score_components", x => x.Id);
                    table.CheckConstraint("CK_score_components_score_value_nonneg", "\"ScoreValue\" >= 0");
                    table.CheckConstraint("CK_score_components_weight_positive", "\"Weight\" > 0");
                    table.ForeignKey(
                        name: "FK_score_components_score_records_ScoreRecordId",
                        column: x => x.ScoreRecordId,
                        principalTable: "score_records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_score_components_ScoreRecordId",
                table: "score_components",
                column: "ScoreRecordId");

            migrationBuilder.CreateIndex(
                name: "IX_score_records_RollNumber",
                table: "score_records",
                column: "RollNumber");

            migrationBuilder.CreateIndex(
                name: "IX_score_records_SubjectClassId",
                table: "score_records",
                column: "SubjectClassId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "score_components");

            migrationBuilder.DropTable(
                name: "score_records");

            migrationBuilder.DropTable(
                name: "subject_classes");
        }
    }
}
