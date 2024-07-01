using MasterInvoice.Models.invoice;
using MasterInvoice.Models.payments;
using MasterInvoice.Models.status;
using Microsoft.Ajax.Utilities;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ContextBase : DbContext
    {
        public ContextBase(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Invoices> Invoices { get; set; }
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

            modelBuilder.Entity<Invoices>(entity =>
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
            string hostIp = "localhost";

            // String de conexão ajustada
            //return $"Data Source={hostIp},1450;Initial Catalog=MASTERINVOICE;User ID=sa;Password=numsey#1234;Connect Timeout=15;Encrypt=False;Trusted_Connection=True;TrustServerCertificate=True";

            return "Data Source=DESKTOP-D47FKN4\\SQLEXPRESS;Initial Catalog=MASTERINVOICE;Integrated Security=True;TrustServerCertificate =True";

        }

    }
}
