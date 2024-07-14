using MasterInvoice.Models.dashBoard;

namespace MasterInvoice.Interfaces
{
    public interface IDashBoard
    {
        Task<DashBoardModel> GetDashboard(DateTime dateInit, DateTime finalDate);
    }
}
