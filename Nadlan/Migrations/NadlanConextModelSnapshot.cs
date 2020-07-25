﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nadlan.Repositories;

namespace Nadlan.Migrations
{
    [DbContext(typeof(NadlanConext))]
    partial class NadlanConextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Nadlan.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountTypeId");

                    b.Property<int>("DisplayOrder");

                    b.Property<bool>("IsIncome");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AccountTypeId");

                    b.ToTable("accounts");
                });

            modelBuilder.Entity("Nadlan.Models.AccountType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("accountTypes","nadlan");
                });

            modelBuilder.Entity("Nadlan.Models.Apartment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<decimal>("CurrentRent");

                    b.Property<string>("Door");

                    b.Property<decimal>("FixedMaintanance");

                    b.Property<int>("Floor");

                    b.Property<DateTime>("PurchaseDate");

                    b.Property<int>("Size");

                    b.Property<short>("Status");

                    b.HasKey("Id");

                    b.ToTable("apartments");
                });

            modelBuilder.Entity("Nadlan.Models.ExpectedTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<decimal>("Amount");

                    b.Property<int>("ApartmentId");

                    b.Property<string>("Comment");

                    b.Property<int>("FrequencyPerYear");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ApartmentId");

                    b.ToTable("expectedTransactions");
                });

            modelBuilder.Entity("Nadlan.Models.Expense", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Hours")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0m);

                    b.Property<int>("TransactionId");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("expenses");
                });

            modelBuilder.Entity("Nadlan.Models.Issues.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApartmentId");

                    b.Property<DateTime?>("DateClose");

                    b.Property<DateTime>("DateOpen");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("Priority");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ApartmentId");

                    b.ToTable("issues");
                });

            modelBuilder.Entity("Nadlan.Models.Message", b =>
                {
                    b.Property<int>("Id");

                    b.Property<DateTime>("DateStamp");

                    b.Property<string>("Description");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsRead");

                    b.Property<int>("IssueId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("IssueId");

                    b.ToTable("messages");
                });

            modelBuilder.Entity("Nadlan.Models.PersonalTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<int>("ApartmentId");

                    b.Property<string>("Comments");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("StakeholderId");

                    b.Property<int>("TransactionType");

                    b.HasKey("Id");

                    b.HasIndex("ApartmentId");

                    b.HasIndex("StakeholderId");

                    b.ToTable("personalTransactions");
                });

            modelBuilder.Entity("Nadlan.Models.Portfolio", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("ApartmentId");

                    b.Property<decimal>("Percentage");

                    b.Property<int>("StakeholderId");

                    b.HasKey("Id");

                    b.HasIndex("ApartmentId");

                    b.HasIndex("StakeholderId");

                    b.ToTable("portfolios");
                });

            modelBuilder.Entity("Nadlan.Models.Renovation.RenovationLine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Category");

                    b.Property<string>("Comments");

                    b.Property<decimal>("Cost");

                    b.Property<bool>("IsCompleted");

                    b.Property<int>("RenovationProjectId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("RenovationProjectId");

                    b.ToTable("lines","renovation");
                });

            modelBuilder.Entity("Nadlan.Models.Renovation.RenovationPayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<string>("Comments");

                    b.Property<string>("Criteria");

                    b.Property<DateTime?>("DatePayment");

                    b.Property<bool>("IsConfirmed");

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("RenovationProjectId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("RenovationProjectId");

                    b.ToTable("payments","renovation");
                });

            modelBuilder.Entity("Nadlan.Models.Renovation.RenovationProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApartmentId");

                    b.Property<string>("Comments");

                    b.Property<DateTime>("DateEnd");

                    b.Property<DateTime>("DateStart");

                    b.Property<string>("Name");

                    b.Property<decimal>("PeneltyPerDay");

                    b.Property<int>("TransactionId");

                    b.HasKey("Id");

                    b.HasIndex("ApartmentId");

                    b.ToTable("projects","renovation");
                });

            modelBuilder.Entity("Nadlan.Models.Security.AppUser", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.HasKey("UserId");

                    b.ToTable("users","secure");
                });

            modelBuilder.Entity("Nadlan.Models.Security.AppUserClaim", b =>
                {
                    b.Property<Guid>("ClaimId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType")
                        .IsRequired();

                    b.Property<string>("ClaimValue")
                        .IsRequired();

                    b.Property<Guid>("UserId");

                    b.HasKey("ClaimId");

                    b.ToTable("userClaims","secure");
                });

            modelBuilder.Entity("Nadlan.Models.Stakeholder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("stakeholders");
                });

            modelBuilder.Entity("Nadlan.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountId");

                    b.Property<decimal>("Amount");

                    b.Property<int>("ApartmentId");

                    b.Property<string>("Comments");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsBusinessExpense");

                    b.Property<bool>("IsConfirmed");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsPurchaseCost");

                    b.Property<int>("PersonalTransactionId");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ApartmentId");

                    b.ToTable("transactions");
                });

            modelBuilder.Entity("Nadlan.Models.Account", b =>
                {
                    b.HasOne("Nadlan.Models.AccountType", "AccountType")
                        .WithMany()
                        .HasForeignKey("AccountTypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nadlan.Models.ExpectedTransaction", b =>
                {
                    b.HasOne("Nadlan.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nadlan.Models.Apartment", "Apartment")
                        .WithMany()
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nadlan.Models.Expense", b =>
                {
                    b.HasOne("Nadlan.Models.Transaction", "Transaction")
                        .WithMany()
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nadlan.Models.Issues.Issue", b =>
                {
                    b.HasOne("Nadlan.Models.Apartment", "Apartment")
                        .WithMany()
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nadlan.Models.Message", b =>
                {
                    b.HasOne("Nadlan.Models.Renovation.RenovationLine")
                        .WithMany("Messages")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nadlan.Models.Issues.Issue", "Issue")
                        .WithMany("Messages")
                        .HasForeignKey("IssueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nadlan.Models.PersonalTransaction", b =>
                {
                    b.HasOne("Nadlan.Models.Apartment", "Apartment")
                        .WithMany()
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nadlan.Models.Stakeholder", "Stakeholder")
                        .WithMany()
                        .HasForeignKey("StakeholderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nadlan.Models.Portfolio", b =>
                {
                    b.HasOne("Nadlan.Models.Apartment", "Apartment")
                        .WithMany()
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nadlan.Models.Stakeholder", "Stakeholder")
                        .WithMany()
                        .HasForeignKey("StakeholderId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nadlan.Models.Renovation.RenovationLine", b =>
                {
                    b.HasOne("Nadlan.Models.Renovation.RenovationProject", "RenovationProject")
                        .WithMany()
                        .HasForeignKey("RenovationProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nadlan.Models.Renovation.RenovationPayment", b =>
                {
                    b.HasOne("Nadlan.Models.Renovation.RenovationProject", "RenovationProject")
                        .WithMany()
                        .HasForeignKey("RenovationProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nadlan.Models.Renovation.RenovationProject", b =>
                {
                    b.HasOne("Nadlan.Models.Apartment", "Apartment")
                        .WithMany()
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Nadlan.Models.Transaction", b =>
                {
                    b.HasOne("Nadlan.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Nadlan.Models.Apartment", "Apartment")
                        .WithMany()
                        .HasForeignKey("ApartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
