using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MasterInvoice.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IInvoice iInvoiceService;
        public DashboardController(IInvoice iExpense)
        {
            this.iInvoiceService = iExpense;
        }
            public async Task<IActionResult> Index(DateTime? filterDateInit, DateTime? filterDateFinal)
        {
            try
            {
                var dateNow = DateTime.Now;
                var initDate = filterDateInit ?? new DateTime(dateNow.Year, dateNow.Month, 1).AddMonths(-12);
                var endDate = filterDateFinal ?? dateNow;

                var dashboardResult = await iInvoiceService.GetDashBoard(initDate, endDate);
                if (dashboardResult == null) {
                    ViewBag.ErrorMessage = "Error retrieving dashboard data: No data found";
                    return View("Error");
                } 
                ViewBag.filterDateInit = initDate;
                ViewBag.filterDateFinal = endDate;

                return View(dashboardResult);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving dashboard data: {ex.Message}";
                return View(); // or return an error view
            }
        }
    }
}
