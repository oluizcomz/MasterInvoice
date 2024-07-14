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
        /// <summary>
        /// Monta os dados do dashboard com indicadores e dados dos gráficos.
        /// Para melhor performance, a consulta dos dados é feita em paralelo.
        /// </summary>
        /// <param name="dateInit">Data inicial para a consulta dos dados.</param>
        /// <param name="finalDate">Data final para a consulta dos dados.</param>
        /// <returns>Retorna um objeto DashBoardModel contendo todos os indicadores e dados dos gráficos.</returns>
        public async Task<DashBoardModel> GetDashboard(DateTime dateInit, DateTime finalDate)
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

            return new DashBoardModel(
                await issuedTask,
                await noChargeTask,
                await latePaymentTask,
                await paymentMadeTask,
                await duePaymentTask,
                await valuesDelinquencyTask,
                await valuesPastDueTask
            );

        }


        private async Task<string> GetLatePayment(DateTime startDate, DateTime endDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
                var totalLatePayment = await context.Invoices
                   .Where(i => i.StatusId == Models.Enums.StatusType.LatePayment && i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
                   .SumAsync(i => (double)i.Amount);

                return totalLatePayment.ToString("F2"); ;
            }
        }
        private async Task<string> GetDuePayment(DateTime startDate, DateTime endDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {

                var totalLatePayment = await context.Invoices
                   .Where(i => (i.StatusId == MasterInvoice.Models.Enums.StatusType.Issued ) && i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
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
                   .Where(i => i.StatusId == MasterInvoice.Models.Enums.StatusType.PaymentMade && i.IssueDate >= startDate && i.IssueDate < endDate.AddMonths(1))
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
        private async Task<IList<GraficModel>> GetValuesDelinquency(DateTime startDate, DateTime endDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
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

        private async Task<IList<GraficModel>> GetValuesPastDue(DateTime startDate, DateTime endDate)
        {
            using (var context = new ContextBase(optionsBuilder))
            {
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
