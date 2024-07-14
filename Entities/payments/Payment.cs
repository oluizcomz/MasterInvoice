using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("payments")]
    public class Payment
    {
        public int Id { get; set; }

        [Display(Name = "ID da Nota Fiscal")]
        public int InvoiceId { get; set; }

        [Display(Name = "Data do Pagamento")]
        public DateTime PaymentDate { get; set; }

        public virtual required Invoice Invoice { get; set; }
    }

}
