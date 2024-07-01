using System.ComponentModel.DataAnnotations;

namespace MasterInvoice.Models.Enums
{
    public enum StatusType
    {
        [Display(Name = "Emitida")]
        Issued = 1,

        [Display(Name = "Cobrança Realizada")]
        Charged = 2,

        [Display(Name = "Pagamento em Atraso")]
        LatePayment = 3,

        [Display(Name = "Pagamento Realizado")]
        PaymentMade = 4
    }
}
