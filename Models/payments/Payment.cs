using MasterInvoice.Models.invoice;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterInvoice.Models.payments
{
    [Table("payments")]
    public class Payment
    {
        public int Id { get; set; }

        [Display(Name = "ID da Nota Fiscal")]
        public int InvoiceId { get; set; }

        [Display(Name = "Data do Pagamento")]
        public DateTime PaymentDate { get; set; }

        public virtual Invoices Invoice { get; set; }
    }

}
