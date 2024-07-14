using Entities;

namespace Domain.Interfaces
{
    public interface IInvoice : IGeneric<Invoice>
    {
        Task<IList<Invoice>> GetFiltered(DateTime? issueDate = null, DateTime? billingDate = null, DateTime? paymentDate = null, string? status = null);
        Task<Invoice> GetByID(int id);
        Task<DashBoardModelView> GetDashBoard(DateTime dateInit, DateTime finalDate);

    }
}
