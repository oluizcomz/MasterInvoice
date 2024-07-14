using Domain.Interfaces;
using Entities;
using Entities.Enums;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository.Generics
{
    public class RepositoryInvoice : RepositoryGenerics<Invoice>, IInvoice
    {
        private readonly DbContextOptions<ContextBase> optionsBuilder;
        public RepositoryInvoice()
        {
            optionsBuilder = new DbContextOptions<ContextBase>();
        }
        public async Task<IList<Invoice>> GetFiltered(DateTime? IssueDate = null, DateTime? BillingDate = null, DateTime? PaymentDate = null, string? status = null)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                IQueryable<Invoice> query = context.Invoices;

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

        public async Task<Invoice> GetByID(int id)
        {
            using (var bd = new ContextBase(optionsBuilder))
            {
                return await bd.Invoices.Where(expense => expense.Id == id)
                    .AsNoTracking()
                    .FirstOrDefaultAsync();
            }
        }
      
        private async Task<string> GetDuePayment(DateTime startDate, DateTime endDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {

                var totalLatePayment = await context.Invoices
                   .Where(i => (i.StatusId == StatusType.Issued) && i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
                   .SumAsync(i => (double)i.Amount);

                return totalLatePayment.ToString("F2"); ;
            }
        }



        private async Task<string> GetNoCharge(DateTime startDate, DateTime endDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var totalWithoutBilling = await context.Invoices
                    .Where(i => i.BillingDate == null && i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
                    .SumAsync(i => (double)i.Amount);

                return totalWithoutBilling.ToString("F2"); ;
            }
        }

        private async Task<string> GetPaymentMade(DateTime startDate, DateTime endDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var totaPaymentMade = await context.Invoices
                   .Where(i => i.StatusId == StatusType.PaymentMade && i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
                   .SumAsync(i => (double)i.Amount);

                return totaPaymentMade.ToString("F2"); ;
            }
        }
        private async Task<string> GetIssued(DateTime startDate, DateTime endDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var totalIssued = await context.Invoices
                    .Where(i => i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
                    .SumAsync(i => (double)i.Amount);

                return totalIssued.ToString("F2"); ;
            }
        }
        private async Task<IList<GraficModelView>> GetValuesDelinquency(DateTime startDate, DateTime endDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var result = await context.Invoices
                    .Where(i => i.StatusId == StatusType.LatePayment && i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
                    .GroupBy(i => new { Year = i.IssueDate.Year, Month = i.IssueDate.Month })
                    .OrderBy(g => g.Key.Year)
                    .ThenBy(g => g.Key.Month)
                    .Select(g => new GraficModelView
                    {
                        Month = $"{g.Key.Month}/{g.Key.Year}",
                        Value = g.Sum(i => i.Amount)
                    })
                    .ToListAsync();

                return result;
            }
        }

        private async Task<IList<GraficModelView>> GetValuesPastDue(DateTime startDate, DateTime endDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var result = await context.Invoices
                    .Where(i => i.StatusId == StatusType.PaymentMade && i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
                    .GroupBy(i => new { Year = i.IssueDate.Year, Month = i.IssueDate.Month })
                    .OrderBy(g => g.Key.Year)
                    .ThenBy(g => g.Key.Month)
                    .Select(g => new GraficModelView
                    {
                        Month = $"{g.Key.Month}/{g.Key.Year}",
                        Value = g.Sum(i => i.Amount)
                    })
                    .ToListAsync();

                return result;
            }
        }

        private async Task<string> GetLatePayment(DateTime startDate, DateTime endDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var totalLatePayment = await context.Invoices
                   .Where(i => i.StatusId == StatusType.LatePayment && i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
                   .SumAsync(i => (double)i.Amount);

                return totalLatePayment.ToString("F2"); ;
            }
        }

        public async Task<DashBoardModelView> GetDashBoard(DateTime dateInit, DateTime finalDate)
        {
            var issuedTask = GetIssued(dateInit, finalDate);
            var noChargeTask = GetNoCharge(dateInit, finalDate);
            var latePaymentTask = GetLatePayment(dateInit, finalDate);
            var paymentMadeTask = GetPaymentMade(dateInit, finalDate);
            var duePaymentTask = GetDuePayment(dateInit, finalDate);
            var valuesDelinquencyTask = GetValuesDelinquency(dateInit, finalDate);
            var valuesPastDueTask = GetValuesPastDue(dateInit, finalDate);

            await Task.WhenAll(issuedTask, noChargeTask, latePaymentTask, paymentMadeTask,
                                duePaymentTask, valuesDelinquencyTask, valuesPastDueTask);

            return new DashBoardModelView
            {
                Issued = await issuedTask,
                NoCharge = await noChargeTask,
                LatePayment = await latePaymentTask,
                DuePayment = await paymentMadeTask,
                PaymentMade = await duePaymentTask,
                ValuesDelinquency = await valuesDelinquencyTask,
                ValuesPastDue = await valuesPastDueTask
            };
        }
    }
}
