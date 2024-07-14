using Humanizer.Localisation;
using Infrastructure.Persistence;
using MasterInvoice.Interfaces;
using MasterInvoice.Models.Enums;
using MasterInvoice.Models.invoice;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Web.WebPages;


namespace MasterInvoice.Services
{
    public class InvoiceService : IInvoice
    {
        private readonly DbContextOptions<ContextBase> optionsBuilder;

        public InvoiceService()
        {
            optionsBuilder = new DbContextOptionsBuilder<ContextBase>().Options;
        }

        public async Task<List<Invoices>> GetAllInvoices(DateTime? IssueDate = null, DateTime? BillingDate = null, DateTime? PaymentDate = null, string? status = null)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                IQueryable<Invoices> query = context.Invoices;

                // Aplicar filtros
                if (IssueDate != null)
                    query = query.Where(i => i.IssueDate >= IssueDate && i.IssueDate < IssueDate.Value.AddMonths(1));

                if (BillingDate != null)
                    query = query.Where(i => i.BillingDate >= BillingDate && i.BillingDate < BillingDate.Value.AddMonths(1));

                if (PaymentDate != null)
                    query = query.Where(i => context.Payments.Any(p => p.InvoiceId == i.Id && p.PaymentDate >= PaymentDate && p.PaymentDate < PaymentDate.Value.AddMonths(1)));

                if (!string.IsNullOrEmpty(status) && Enum.TryParse<StatusType>(status, out StatusType statusEnum))
                    query = query.Where(i => i.StatusId == statusEnum);

                return await query.ToListAsync();
            }
        }

        public async Task<Invoices> GetInvoiceById(int id)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                return await context.Invoices.FirstOrDefaultAsync(i => i.Id == id);
            }
        }

        public async Task AddInvoice(Invoices invoice)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                await context.Invoices.AddAsync(invoice);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateInvoice(Invoices invoice)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                context.Invoices.Update(invoice);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteInvoice(int id)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var invoice = await context.Invoices.FirstOrDefaultAsync(i => i.Id == id);
                if (invoice != null)
                {
                    context.Invoices.Remove(invoice);
                    await context.SaveChangesAsync();
                }
            }
        }
      
    }
}
