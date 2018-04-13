using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Productor.Migrations
{
    public partial class UpdateOrderConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "No",
                table: "OrderHeaders");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "OrderDetails");

            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "OrderDetails",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "No",
                table: "OrderHeaders",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OrderNo",
                table: "OrderDetails",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
