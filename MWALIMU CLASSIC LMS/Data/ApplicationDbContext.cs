using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MWALIMU_CLASSIC_LMS.Models;

namespace MWALIMU_CLASSIC_LMS.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<CreditScore> CreditScores { get; set; }
        public DbSet<LoanOfficer> LoanOfficers { get; set; }
        public DbSet<Collateral> Collaterals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure one-to-one relationship between Customer and CreditScore
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.CreditScore)
                .WithOne(cs => cs.Customer)
                .HasForeignKey<CreditScore>(cs => cs.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-one relationship between Loan and Collateral
            modelBuilder.Entity<Loan>()
                .HasOne(l => l.Collateral)
                .WithOne(c => c.Loan)
                .HasForeignKey<Collateral>(c => c.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Customer and Account
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Accounts)
                .WithOne(a => a.Customer)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between Customer and Loan
            // Changed to Restrict to avoid multiple cascade paths
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Loans)
                .WithOne(l => l.Customer)
                .HasForeignKey(l => l.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between Loan and Payment
            modelBuilder.Entity<Loan>()
                .HasMany(l => l.Payments)
                .WithOne(p => p.Loan)
                .HasForeignKey(p => p.LoanId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-many relationship between LoanOfficer and Loan
            // Keep this as SetNull
            modelBuilder.Entity<LoanOfficer>()
                .HasMany(lo => lo.ReviewedLoans)
                .WithOne(l => l.LoanOfficer)
                .HasForeignKey(l => l.LoanOfficerId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure one-to-one relationship between ApplicationUser and Customer
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Customer)
                .WithOne(c => c.ApplicationUser)
                .HasForeignKey<Customer>(c => c.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure one-to-one relationship between ApplicationUser and LoanOfficer
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.LoanOfficer)
                .WithOne(lo => lo.ApplicationUser)
                .HasForeignKey<LoanOfficer>(lo => lo.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add indexes for better query performance
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();

            modelBuilder.Entity<LoanOfficer>()
                .HasIndex(lo => lo.Email)
                .IsUnique();
        }
    }
}
