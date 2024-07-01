using MasterInvoice.Models.invoice;

namespace MasterInvoice.Interfaces
{
    public interface IInvoice
    {
        Task<List<Invoices>> GetAllInvoices(DateTime? StatusType = null, DateTime? BillingDate = null, DateTime? PaymentDate = null, string? status = null);
        Task<Invoices> GetInvoiceById(int id);
        Task AddInvoice(Invoices invoice);
        Task UpdateInvoice(Invoices invoice);
        Task DeleteInvoice(int id);
    }
}
