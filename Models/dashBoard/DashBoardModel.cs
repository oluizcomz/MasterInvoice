using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MasterInvoice.Models.dashBoard
{
    public class DashBoardModel
    {
        public DashBoardModel(string issued, string noCharge, string latePayment, string duePayment, string paymentMade, IList<GraficModel> valuesDelinquency, IList<GraficModel> valuesPastDue)
        {
            Issued = issued;
            NoCharge = noCharge;
            LatePayment = latePayment;
            DuePayment = duePayment;
            PaymentMade = paymentMade;
            ValuesDelinquency = valuesDelinquency;
            ValuesPastDue = valuesPastDue;
        }
        [Display(Name = "Notas Emitidas")]
        public string Issued { get; set; }

        [Display(Name = "Notas sem cobrança")]
        public string NoCharge { get; set; }
        
        [Display(Name = "Notas vencidas")]
        public string LatePayment { get; set; }
         
        [Display(Name = "Notas a vencer")]
        public string DuePayment { get; set; }
        
        [Display(Name = "Notas a pagas")]
        public string PaymentMade { get; set; }

        [Display(Name = "Inadimplência")]
        public IList<GraficModel> ValuesDelinquency { get; set; }

        [Display(Name = "Receita Recebida")]
        public IList<GraficModel> ValuesPastDue { get; set; }
    }
}
