using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Nadlan.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "renovation");

            migrationBuilder.EnsureSchema(
                name: "secure");

            migrationBuilder.CreateTable(
                name: "accountTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "apartments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    Status = table.Column<short>(nullable: false),
                    PurchaseDate = table.Column<DateTime>(nullable: false),
                    Floor = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    Door = table.Column<string>(nullable: true),
                    CurrentRent = table.Column<decimal>(nullable: false),
                    FixedMaintanance = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apartments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    DateStamp = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stakeholders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stakeholders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "userClaims",
                schema: "secure",
                columns: table => new
                {
                    ClaimId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    ClaimType = table.Column<string>(nullable: false),
                    ClaimValue = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userClaims", x => x.ClaimId);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "secure",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(maxLength: 255, nullable: false),
                    Password = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsIncome = table.Column<bool>(nullable: false),
                    AccountTypeId = table.Column<int>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_accounts_accountTypes_AccountTypeId",
                        column: x => x.AccountTypeId,
                        principalTable: "accountTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "issues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Priority = table.Column<int>(nullable: false),
                    DateOpen = table.Column<DateTime>(nullable: false),
                    DateClose = table.Column<DateTime>(nullable: true),
                    ApartmentId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_issues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_issues_apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "projects",
                schema: "renovation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    ApartmentId = table.Column<int>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false),
                    PeneltyPerDay = table.Column<decimal>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    TransactionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_projects_apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "personalTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    TransactionType = table.Column<int>(nullable: false),
                    StakeholderId = table.Column<int>(nullable: false),
                    ApartmentId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_personalTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_personalTransactions_apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_personalTransactions_stakeholders_StakeholderId",
                        column: x => x.StakeholderId,
                        principalTable: "stakeholders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "portfolios",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    ApartmentId = table.Column<int>(nullable: false),
                    StakeholderId = table.Column<int>(nullable: false),
                    Percentage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_portfolios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_portfolios_apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_portfolios_stakeholders_StakeholderId",
                        column: x => x.StakeholderId,
                        principalTable: "stakeholders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expectedTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ApartmentId = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    Amount = table.Column<decimal>(nullable: false),
                    FrequencyPerYear = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expectedTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expectedTransactions_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_expectedTransactions_apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsPurchaseCost = table.Column<bool>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    IsBusinessExpense = table.Column<bool>(nullable: false),
                    IsConfirmed = table.Column<bool>(nullable: false),
                    ApartmentId = table.Column<int>(nullable: false),
                    AccountId = table.Column<int>(nullable: false),
                    PersonalTransactionId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_transactions_apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lines",
                schema: "renovation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Category = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true),
                    Cost = table.Column<decimal>(nullable: false),
                    RenovationProjectId = table.Column<int>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_lines_projects_RenovationProjectId",
                        column: x => x.RenovationProjectId,
                        principalSchema: "renovation",
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "renovation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    DatePayment = table.Column<DateTime>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    Criteria = table.Column<string>(nullable: true),
                    Comments = table.Column<string>(nullable: true),
                    IsConfirmed = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    RenovationProjectId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_payments_projects_RenovationProjectId",
                        column: x => x.RenovationProjectId,
                        principalSchema: "renovation",
                        principalTable: "projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "expenses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionId = table.Column<int>(nullable: false),
                    Hours = table.Column<decimal>(nullable: false, defaultValue: 0m)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_expenses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_expenses_transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_AccountTypeId",
                table: "accounts",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_expectedTransactions_AccountId",
                table: "expectedTransactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_expectedTransactions_ApartmentId",
                table: "expectedTransactions",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_expenses_TransactionId",
                table: "expenses",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_issues_ApartmentId",
                table: "issues",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_personalTransactions_ApartmentId",
                table: "personalTransactions",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_personalTransactions_StakeholderId",
                table: "personalTransactions",
                column: "StakeholderId");

            migrationBuilder.CreateIndex(
                name: "IX_portfolios_ApartmentId",
                table: "portfolios",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_portfolios_StakeholderId",
                table: "portfolios",
                column: "StakeholderId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_AccountId",
                table: "transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_ApartmentId",
                table: "transactions",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_lines_RenovationProjectId",
                schema: "renovation",
                table: "lines",
                column: "RenovationProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_RenovationProjectId",
                schema: "renovation",
                table: "payments",
                column: "RenovationProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_projects_ApartmentId",
                schema: "renovation",
                table: "projects",
                column: "ApartmentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "expectedTransactions");

            migrationBuilder.DropTable(
                name: "expenses");

            migrationBuilder.DropTable(
                name: "issues");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "personalTransactions");

            migrationBuilder.DropTable(
                name: "portfolios");

            migrationBuilder.DropTable(
                name: "lines",
                schema: "renovation");

            migrationBuilder.DropTable(
                name: "payments",
                schema: "renovation");

            migrationBuilder.DropTable(
                name: "userClaims",
                schema: "secure");

            migrationBuilder.DropTable(
                name: "users",
                schema: "secure");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "stakeholders");

            migrationBuilder.DropTable(
                name: "projects",
                schema: "renovation");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "apartments");

            migrationBuilder.DropTable(
                name: "accountTypes");
        }
    }
}
