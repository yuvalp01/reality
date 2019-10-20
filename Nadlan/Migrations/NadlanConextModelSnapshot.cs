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

                    b.Property<bool>("IsIncome");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("AccountTypeId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Nadlan.Models.AccountType", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("AccountTypes");
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

                    b.ToTable("Apartments");
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

                    b.Property<int>("PrequencyPerYear");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ApartmentId");

                    b.ToTable("ExpectedTransactions");
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

                    b.ToTable("Expenses");
                });

            modelBuilder.Entity("Nadlan.Models.PersonalTransaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<string>("Comments");

                    b.Property<DateTime>("Date");

                    b.Property<int>("StakeholderId");

                    b.HasKey("Id");

                    b.HasIndex("StakeholderId");

                    b.ToTable("PersonalTransactions");
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

                    b.ToTable("Portfolios");
                });

            modelBuilder.Entity("Nadlan.Models.Renovation.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description");

                    b.Property<int?>("LineId");

                    b.Property<int?>("ProductId");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(1);

                    b.HasKey("Id");

                    b.HasIndex("LineId");

                    b.HasIndex("ProductId");

                    b.ToTable("Items","renovation");
                });

            modelBuilder.Entity("Nadlan.Models.Renovation.Line", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApartmentId");

                    b.Property<int>("Category");

                    b.Property<string>("Comments");

                    b.Property<string>("Title");

                    b.Property<decimal>("WorkCost");

                    b.HasKey("Id");

                    b.HasIndex("ApartmentId");

                    b.ToTable("Lines","renovation");
                });

            modelBuilder.Entity("Nadlan.Models.Renovation.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Link");

                    b.Property<string>("Name");

                    b.Property<decimal>("Price")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(0m);

                    b.Property<string>("Reference");

                    b.HasKey("Id");

                    b.ToTable("Products","renovation");
                });

            modelBuilder.Entity("Nadlan.Models.Stakeholder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Stakeholders");
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

                    b.Property<bool>("IsPurchaseCost");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ApartmentId");

                    b.ToTable("Transactions");
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

            modelBuilder.Entity("Nadlan.Models.PersonalTransaction", b =>
                {
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

            modelBuilder.Entity("Nadlan.Models.Renovation.Item", b =>
                {
                    b.HasOne("Nadlan.Models.Renovation.Line")
                        .WithMany("Items")
                        .HasForeignKey("LineId");

                    b.HasOne("Nadlan.Models.Renovation.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId");
                });

            modelBuilder.Entity("Nadlan.Models.Renovation.Line", b =>
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
