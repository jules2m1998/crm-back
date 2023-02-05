﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.App.API.Migrations
{
    /// <inheritdoc />
    public partial class userparent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CreatorId",
                table: "AspNetUsers",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CreatorId",
                table: "AspNetUsers",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_CreatorId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CreatorId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AspNetUsers");
        }
    }
}
