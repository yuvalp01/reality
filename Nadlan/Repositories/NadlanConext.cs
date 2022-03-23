using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using System;
using Nadlan.Models.Renovation;
using Nadlan.Models.Security;
using Nadlan.Models.Issues;

namespace Nadlan.Repositories
{
    public class NadlanConext : DbContext
    {
        public NadlanConext(DbContextOptions<NadlanConext> options) : base(options) 
        {

        }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<ExpectedTransaction> ExpectedTransactions { get; set; }
        public DbSet<PersonalTransaction> PersonalTransactions { get; set; }
        public DbSet<Stakeholder> Stakeholders { get; set; }
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<AppUserClaim> Claims { get; set; }
        public DbSet<RenovationProject> RenovationProjects { get; set; }
        public DbSet<RenovationLine> RenovationLines { get; set; }
        public DbSet<RenovationPayment> RenovationPayments { get; set; }
        public DbSet<RenovationProduct> RenovationProducts { get; set; }
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .Property(a => a.Hours)
                .HasDefaultValue(0);
            modelBuilder.Entity<Issue>()
                .Property(a => a.IsNew)
                .HasDefaultValue(true);
            modelBuilder.Entity<Event>()
                .Property(a => a.Id)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Event>()
                .Property(a => a.Date)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Event>()
                .Property(a => a.DateCreated)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<Lesson>()
                .Property(a => a.Id)
                .HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Lesson>()
                .Property(a => a.Date)
                .HasDefaultValueSql("GETDATE()");
            modelBuilder.Entity<Lesson>()
                .Property(a => a.DateCreated)
                .HasDefaultValueSql("GETDATE()");
            //modelBuilder.Entity<Item>()
            //    .Property(a => a.Quantity)
            //    .HasDefaultValue(1);
        }
    }
}
