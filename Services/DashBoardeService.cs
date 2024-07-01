using Infrastructure.Persistence;
using MasterInvoice.Interfaces;
using MasterInvoice.Models.dashBoard;
using Microsoft.EntityFrameworkCore;

namespace MasterInvoice.Services
{
    public class DashBoardeService : IDashBoard
    {
        private readonly DbContextOptions<ContextBase> optionsBuilder;
        public DashBoardeService()
        {
            optionsBuilder = new DbContextOptions<ContextBase>();
        }
        public async Task<DashBoardModel> GetDashboard(string dateInit, string finalDate)
        {
           DashBoardModel dashBoardModel = new DashBoardModel();

           dashBoardModel.Issued = await GetIssued(dateInit, finalDate);
           dashBoardModel.NoCharge = await GetNoCharge(dateInit, finalDate);
           dashBoardModel.LatePayment = await GetLatePayment(dateInit, finalDate);
           dashBoardModel.PaymentMade = await GetPaymentMade(dateInit, finalDate);
           dashBoardModel.DuePayment = await GetDuePayment(dateInit, finalDate);
           dashBoardModel.ValuesDelinquency = await GetValuesDelinquency(dateInit, finalDate);
           dashBoardModel.ValuesPastDue = await GetValuesPastDue(dateInit, finalDate);

            return dashBoardModel;
        }

        private async Task<double> GetLatePayment(string dateInit, string finalDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var startDate = DateTime.Parse(dateInit);
                var endDate = DateTime.Parse(finalDate);

                var totalLatePayment = await context.Invoices
                   .Where(i => i.StatusId == Models.Enums.StatusType.LatePayment && i.IssueDate >= startDate && i.IssueDate <= endDate)
                   .SumAsync(i => (double)i.Amount);

                return totalLatePayment;
            }
        }
        private async Task<double> GetDuePayment(string dateInit, string finalDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var startDate = DateTime.Parse(dateInit);
                var endDate = DateTime.Parse(finalDate);

                var totalLatePayment = await context.Invoices
                   .Where(i => (i.StatusId == MasterInvoice.Models.Enums.StatusType.Issued ) && i.IssueDate >= startDate && i.IssueDate <= endDate)
                   .SumAsync(i => (double)i.Amount);

                return totalLatePayment;
            }
        }



        private async Task<double> GetNoCharge(string dateInit, string finalDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var startDate = DateTime.Parse(dateInit);
                var endDate = DateTime.Parse(finalDate);

                var totalWithoutBilling = await context.Invoices
                    .Where(i => i.BillingDate == null && i.IssueDate >= startDate && i.IssueDate <= endDate)
                    .SumAsync(i => (double)i.Amount);

                return totalWithoutBilling;
            }
        }

        private async Task<double> GetPaymentMade(string dateInit, string finalDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var startDate = DateTime.Parse(dateInit);
                var endDate = DateTime.Parse(finalDate);

                var totaPaymentMade = await context.Invoices
                   .Where(i => i.StatusId == MasterInvoice.Models.Enums.StatusType.PaymentMade && i.IssueDate >= startDate && i.IssueDate <= endDate)
                   .SumAsync(i => (double)i.Amount);

                return totaPaymentMade;
            }
        }
        private async Task<double> GetIssued(string dateInit, string finalDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var startDate = DateTime.Parse(dateInit);
                var endDate = DateTime.Parse(finalDate);

                var totalIssued = await context.Invoices
                    .Where(i => i.IssueDate >= startDate && i.IssueDate <= endDate)
                    .SumAsync(i => (double)i.Amount);

                return totalIssued;
            }
        }
        private async Task<IList<GraficModel>> GetValuesDelinquency(string dateInit, string finalDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var startDate = DateTime.Parse(dateInit);
                var endDate = DateTime.Parse(finalDate);

                var result = await context.Invoices
                    .Where(i => i.StatusId == Models.Enums.StatusType.LatePayment && i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
                    .GroupBy(i => new { Year = i.IssueDate.Year, Month = i.IssueDate.Month })
                    .OrderBy(g => g.Key.Year)
                    .ThenBy(g => g.Key.Month)
                    .Select(g => new GraficModel
                    {
                        Month = $"{g.Key.Month}/{g.Key.Year}",
                        Value = g.Sum(i => i.Amount)
                    })
                    .ToListAsync();

                return result;
            }
        }

        private async Task<IList<GraficModel>> GetValuesPastDue(string dateInit, string finalDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var startDate = DateTime.Parse(dateInit);
                var endDate = DateTime.Parse(finalDate);

                var result = await context.Invoices
                 .Where(i => i.StatusId == MasterInvoice.Models.Enums.StatusType.PaymentMade && i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
                 .GroupBy(i => new { Year = i.IssueDate.Year, Month = i.IssueDate.Month })
                 .OrderBy(g => g.Key.Year)
                 .ThenBy(g => g.Key.Month)
                 .Select(g => new GraficModel
                 {
                     Month = $"{g.Key.Month}/{g.Key.Year}",
                     Value = g.Sum(i => i.Amount)
                 }).ToListAsync();

                return result;
            }
        }
    }
}
