using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MasterInvoice.Models.DashBoardModelView
{
    public class DashBoardModelView
    {
       
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
        public IList<GraficModelView> ValuesDelinquency { get; set; }

        [Display(Name = "Receita Recebida")]
        public IList<GraficModelView> ValuesPastDue { get; set; }
    }
}
