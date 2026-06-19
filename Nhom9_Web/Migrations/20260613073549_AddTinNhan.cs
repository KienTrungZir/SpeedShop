using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nhom9_Web.Migrations
{
    public partial class AddTinNhan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TinNhans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NguoiGuiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NguoiNhanId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    DaDoc = table.Column<bool>(type: "bit", nullable: false),
                    NgayGui = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinNhans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TinNhans_AspNetUsers_NguoiGuiId",
                        column: x => x.NguoiGuiId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TinNhans_AspNetUsers_NguoiNhanId",
                        column: x => x.NguoiNhanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_NguoiGuiId_NguoiNhanId_NgayGui",
                table: "TinNhans",
                columns: new[] { "NguoiGuiId", "NguoiNhanId", "NgayGui" });

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_NguoiNhanId",
                table: "TinNhans",
                column: "NguoiNhanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TinNhans");
        }
    }
}
