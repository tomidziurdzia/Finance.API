using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceApp.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class TransferInvestments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "InvestmentAccountId",
                table: "Incomes",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "InvestmentAccountId",
                table: "Expenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_InvestmentAccountId",
                table: "Incomes",
                column: "InvestmentAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_InvestmentAccountId",
                table: "Expenses",
                column: "InvestmentAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_InvestmentAccounts_InvestmentAccountId",
                table: "Expenses",
                column: "InvestmentAccountId",
                principalTable: "InvestmentAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_InvestmentAccounts_InvestmentAccountId",
                table: "Incomes",
                column: "InvestmentAccountId",
                principalTable: "InvestmentAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_InvestmentAccounts_InvestmentAccountId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_InvestmentAccounts_InvestmentAccountId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_InvestmentAccountId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_InvestmentAccountId",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "InvestmentAccountId",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "InvestmentAccountId",
                table: "Expenses");
        }
    }
}
