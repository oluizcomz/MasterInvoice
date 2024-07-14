using Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class ContextBase : DbContext
    {
        public ContextBase(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer(ObterStringConexao());
                base.OnConfiguring(optionsBuilder);
            }
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description)
                      .IsRequired()
                      .HasMaxLength(255);
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PayerName)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(e => e.IdentificationNumber)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(e => e.IssueDate)
                      .IsRequired();
                entity.Property(e => e.Amount)
                      .IsRequired()
                      .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InvoiceDoc)
                      .HasMaxLength(255);
                entity.Property(e => e.BillDoc)
                      .HasMaxLength(255);

                entity.Property(e => e.StatusId)
                      .IsRequired()
                      .HasMaxLength(255); 
            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.InvoiceId)
                      .IsRequired();
                entity.Property(e => e.PaymentDate)
                      .IsRequired();
            });

        }


        public string ObterStringConexao()
        {
            var server = "sqldata";
            var port = "1433"; // Default SQL Server port
            var user =  "SA"; // Warning do not use the SA account
            var password = "numsey#1234";
            var database = "MASTERINVOICE";
            var connectionString = $"Server={server},{port};Initial Catalog={database};User ID={user};Password={password}; Integrated Security=False;TrustServerCertificate=True";
            //return "Data Source=DESKTOP-D47FKN4\\SQLEXPRESS;Initial Catalog=MASTERINVOICE;Integrated Security=True;TrustServerCertificate =True";
            return connectionString;
            //return "Data Source=DESKTOP-D47FKN4\\SQLEXPRESS;Initial Catalog=MASTERINVOICE;Integrated Security=True;TrustServerCertificate =True";
        }

    }
}
