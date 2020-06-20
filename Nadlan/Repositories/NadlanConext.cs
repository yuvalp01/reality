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
        //public DbSet<Line> Lines { get; set; }
        //public DbSet<Item> Items { get; set; }
        //public DbSet<Product> Products { get; set; }
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
        public DbSet<Issue> Issues { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Expense>()
                .Property(a => a.Hours)
                .HasDefaultValue(0);
            //modelBuilder.Entity<Product>()
            //    .Property(a => a.Price)
            //    .HasDefaultValue(0);
            //modelBuilder.Entity<Item>()
            //    .Property(a => a.Quantity)
            //    .HasDefaultValue(1);
        }
    }
}
