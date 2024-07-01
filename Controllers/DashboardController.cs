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
        public async Task<IActionResult> Index()
        {
            try
            {
                var dateNow = DateTime.Now;
                var initDate = dateNow.AddMonths(-12);
                string finalDate = dateNow.ToString("MM/yyyy");
                string dateInit = initDate.ToString("MM/yyyy");

                var dashboardData = await _dashboardService.GetDashboard(dateInit, finalDate);
                return View(dashboardData);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving dashboard data: {ex.Message}";
                return View(); // or return an error view
            }
        }
        [HttpGet("GetDashboard")]
        public async Task<ActionResult<DashBoardModel>> GetDashboard(string dateInit, string finalDate)
        {
            try
            {
                var dashboardData = await _dashboardService.GetDashboard(dateInit, finalDate);
                return Ok(dashboardData);
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
