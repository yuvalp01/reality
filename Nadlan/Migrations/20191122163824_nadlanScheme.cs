using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class nadlanScheme : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AccountTypes_AccountTypeId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpectedTransactions_Accounts_AccountId",
                table: "ExpectedTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpectedTransactions_Apartments_ApartmentId",
                table: "ExpectedTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Expenses_Transactions_TransactionId",
                table: "Expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTransactions_Apartments_ApartmentId",
                table: "PersonalTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonalTransactions_Stakeholders_StakeholderId",
                table: "PersonalTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_Apartments_ApartmentId",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Portfolios_Stakeholders_StakeholderId",
                table: "Portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Apartments_ApartmentId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Apartments_ApartmentId",
                schema: "renovation",
                table: "Lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stakeholders",
                table: "Stakeholders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Portfolios",
                table: "Portfolios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonalTransactions",
                table: "PersonalTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExpectedTransactions",
                table: "ExpectedTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Apartments",
                table: "Apartments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountTypes",
                table: "AccountTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.EnsureSchema(
                name: "nadlan");

            migrationBuilder.RenameTable(
                name: "Transactions",
                newName: "transactions",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "Stakeholders",
                newName: "stakeholders",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "Portfolios",
                newName: "portfolios",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "PersonalTransactions",
                newName: "personalTransactions",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "Expenses",
                newName: "expenses",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "ExpectedTransactions",
                newName: "expectedTransactions",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "Apartments",
                newName: "apartments",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "AccountTypes",
                newName: "accountTypes",
                newSchema: "nadlan");

            migrationBuilder.RenameTable(
                name: "Accounts",
                newName: "accounts",
                newSchema: "nadlan");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_ApartmentId",
                schema: "nadlan",
                table: "transactions",
                newName: "IX_transactions_ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_AccountId",
                schema: "nadlan",
                table: "transactions",
                newName: "IX_transactions_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Portfolios_StakeholderId",
                schema: "nadlan",
                table: "portfolios",
                newName: "IX_portfolios_StakeholderId");

            migrationBuilder.RenameIndex(
                name: "IX_Portfolios_ApartmentId",
                schema: "nadlan",
                table: "portfolios",
                newName: "IX_portfolios_ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalTransactions_StakeholderId",
                schema: "nadlan",
                table: "personalTransactions",
                newName: "IX_personalTransactions_StakeholderId");

            migrationBuilder.RenameIndex(
                name: "IX_PersonalTransactions_ApartmentId",
                schema: "nadlan",
                table: "personalTransactions",
                newName: "IX_personalTransactions_ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Expenses_TransactionId",
                schema: "nadlan",
                table: "expenses",
                newName: "IX_expenses_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpectedTransactions_ApartmentId",
                schema: "nadlan",
                table: "expectedTransactions",
                newName: "IX_expectedTransactions_ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_ExpectedTransactions_AccountId",
                schema: "nadlan",
                table: "expectedTransactions",
                newName: "IX_expectedTransactions_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Accounts_AccountTypeId",
                schema: "nadlan",
                table: "accounts",
                newName: "IX_accounts_AccountTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_transactions",
                schema: "nadlan",
                table: "transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_stakeholders",
                schema: "nadlan",
                table: "stakeholders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_portfolios",
                schema: "nadlan",
                table: "portfolios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_personalTransactions",
                schema: "nadlan",
                table: "personalTransactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_expenses",
                schema: "nadlan",
                table: "expenses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_expectedTransactions",
                schema: "nadlan",
                table: "expectedTransactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_apartments",
                schema: "nadlan",
                table: "apartments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accountTypes",
                schema: "nadlan",
                table: "accountTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_accounts",
                schema: "nadlan",
                table: "accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_accounts_accountTypes_AccountTypeId",
                schema: "nadlan",
                table: "accounts",
                column: "AccountTypeId",
                principalSchema: "nadlan",
                principalTable: "accountTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_expectedTransactions_accounts_AccountId",
                schema: "nadlan",
                table: "expectedTransactions",
                column: "AccountId",
                principalSchema: "nadlan",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_expectedTransactions_apartments_ApartmentId",
                schema: "nadlan",
                table: "expectedTransactions",
                column: "ApartmentId",
                principalSchema: "nadlan",
                principalTable: "apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_expenses_transactions_TransactionId",
                schema: "nadlan",
                table: "expenses",
                column: "TransactionId",
                principalSchema: "nadlan",
                principalTable: "transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_personalTransactions_apartments_ApartmentId",
                schema: "nadlan",
                table: "personalTransactions",
                column: "ApartmentId",
                principalSchema: "nadlan",
                principalTable: "apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_personalTransactions_stakeholders_StakeholderId",
                schema: "nadlan",
                table: "personalTransactions",
                column: "StakeholderId",
                principalSchema: "nadlan",
                principalTable: "stakeholders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_portfolios_apartments_ApartmentId",
                schema: "nadlan",
                table: "portfolios",
                column: "ApartmentId",
                principalSchema: "nadlan",
                principalTable: "apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_portfolios_stakeholders_StakeholderId",
                schema: "nadlan",
                table: "portfolios",
                column: "StakeholderId",
                principalSchema: "nadlan",
                principalTable: "stakeholders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_accounts_AccountId",
                schema: "nadlan",
                table: "transactions",
                column: "AccountId",
                principalSchema: "nadlan",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_apartments_ApartmentId",
                schema: "nadlan",
                table: "transactions",
                column: "ApartmentId",
                principalSchema: "nadlan",
                principalTable: "apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_apartments_ApartmentId",
                schema: "renovation",
                table: "Lines",
                column: "ApartmentId",
                principalSchema: "nadlan",
                principalTable: "apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_accounts_accountTypes_AccountTypeId",
                schema: "nadlan",
                table: "accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_expectedTransactions_accounts_AccountId",
                schema: "nadlan",
                table: "expectedTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_expectedTransactions_apartments_ApartmentId",
                schema: "nadlan",
                table: "expectedTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_expenses_transactions_TransactionId",
                schema: "nadlan",
                table: "expenses");

            migrationBuilder.DropForeignKey(
                name: "FK_personalTransactions_apartments_ApartmentId",
                schema: "nadlan",
                table: "personalTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_personalTransactions_stakeholders_StakeholderId",
                schema: "nadlan",
                table: "personalTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_portfolios_apartments_ApartmentId",
                schema: "nadlan",
                table: "portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_portfolios_stakeholders_StakeholderId",
                schema: "nadlan",
                table: "portfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_accounts_AccountId",
                schema: "nadlan",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_transactions_apartments_ApartmentId",
                schema: "nadlan",
                table: "transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Lines_apartments_ApartmentId",
                schema: "renovation",
                table: "Lines");

            migrationBuilder.DropPrimaryKey(
                name: "PK_transactions",
                schema: "nadlan",
                table: "transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_stakeholders",
                schema: "nadlan",
                table: "stakeholders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_portfolios",
                schema: "nadlan",
                table: "portfolios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_personalTransactions",
                schema: "nadlan",
                table: "personalTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_expenses",
                schema: "nadlan",
                table: "expenses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_expectedTransactions",
                schema: "nadlan",
                table: "expectedTransactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_apartments",
                schema: "nadlan",
                table: "apartments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accountTypes",
                schema: "nadlan",
                table: "accountTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_accounts",
                schema: "nadlan",
                table: "accounts");

            migrationBuilder.RenameTable(
                name: "transactions",
                schema: "nadlan",
                newName: "Transactions");

            migrationBuilder.RenameTable(
                name: "stakeholders",
                schema: "nadlan",
                newName: "Stakeholders");

            migrationBuilder.RenameTable(
                name: "portfolios",
                schema: "nadlan",
                newName: "Portfolios");

            migrationBuilder.RenameTable(
                name: "personalTransactions",
                schema: "nadlan",
                newName: "PersonalTransactions");

            migrationBuilder.RenameTable(
                name: "expenses",
                schema: "nadlan",
                newName: "Expenses");

            migrationBuilder.RenameTable(
                name: "expectedTransactions",
                schema: "nadlan",
                newName: "ExpectedTransactions");

            migrationBuilder.RenameTable(
                name: "apartments",
                schema: "nadlan",
                newName: "Apartments");

            migrationBuilder.RenameTable(
                name: "accountTypes",
                schema: "nadlan",
                newName: "AccountTypes");

            migrationBuilder.RenameTable(
                name: "accounts",
                schema: "nadlan",
                newName: "Accounts");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_ApartmentId",
                table: "Transactions",
                newName: "IX_Transactions_ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_transactions_AccountId",
                table: "Transactions",
                newName: "IX_Transactions_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_portfolios_StakeholderId",
                table: "Portfolios",
                newName: "IX_Portfolios_StakeholderId");

            migrationBuilder.RenameIndex(
                name: "IX_portfolios_ApartmentId",
                table: "Portfolios",
                newName: "IX_Portfolios_ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_personalTransactions_StakeholderId",
                table: "PersonalTransactions",
                newName: "IX_PersonalTransactions_StakeholderId");

            migrationBuilder.RenameIndex(
                name: "IX_personalTransactions_ApartmentId",
                table: "PersonalTransactions",
                newName: "IX_PersonalTransactions_ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_expenses_TransactionId",
                table: "Expenses",
                newName: "IX_Expenses_TransactionId");

            migrationBuilder.RenameIndex(
                name: "IX_expectedTransactions_ApartmentId",
                table: "ExpectedTransactions",
                newName: "IX_ExpectedTransactions_ApartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_expectedTransactions_AccountId",
                table: "ExpectedTransactions",
                newName: "IX_ExpectedTransactions_AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_accounts_AccountTypeId",
                table: "Accounts",
                newName: "IX_Accounts_AccountTypeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stakeholders",
                table: "Stakeholders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portfolios",
                table: "Portfolios",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonalTransactions",
                table: "PersonalTransactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Expenses",
                table: "Expenses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExpectedTransactions",
                table: "ExpectedTransactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Apartments",
                table: "Apartments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountTypes",
                table: "AccountTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AccountTypes_AccountTypeId",
                table: "Accounts",
                column: "AccountTypeId",
                principalTable: "AccountTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpectedTransactions_Accounts_AccountId",
                table: "ExpectedTransactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpectedTransactions_Apartments_ApartmentId",
                table: "ExpectedTransactions",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Expenses_Transactions_TransactionId",
                table: "Expenses",
                column: "TransactionId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTransactions_Apartments_ApartmentId",
                table: "PersonalTransactions",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonalTransactions_Stakeholders_StakeholderId",
                table: "PersonalTransactions",
                column: "StakeholderId",
                principalTable: "Stakeholders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_Apartments_ApartmentId",
                table: "Portfolios",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Portfolios_Stakeholders_StakeholderId",
                table: "Portfolios",
                column: "StakeholderId",
                principalTable: "Stakeholders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Apartments_ApartmentId",
                table: "Transactions",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Apartments_ApartmentId",
                schema: "renovation",
                table: "Lines",
                column: "ApartmentId",
                principalTable: "Apartments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
