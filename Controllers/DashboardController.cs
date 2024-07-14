using Humanizer.Localisation;
using MasterInvoice.Interfaces;
using MasterInvoice.Models.dashBoard;
using MasterInvoice.Services;
using Microsoft.AspNetCore.Mvc;

namespace MasterInvoice.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashBoard _dashboardService;

        public DashboardController(IDashBoard dashboardService)
        {
            _dashboardService = dashboardService;
        }
        public async Task<IActionResult> Index(DateTime? filterDateInit, DateTime? filterDateFinal)
        {
            try
            {
                var dateNow = DateTime.Now;
                var initDate = filterDateInit ?? new DateTime(dateNow.Year, dateNow.Month, 1).AddMonths(-12);
                var endDate = filterDateFinal ?? dateNow;

                var dashboardData = await _dashboardService.GetDashboard(initDate, endDate);
                
                ViewBag.filterDateInit = initDate;
                ViewBag.filterDateFinal = endDate;

                return View(dashboardData);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving dashboard data: {ex.Message}";
                return View(); // or return an error view
            }
        }
    }
}
