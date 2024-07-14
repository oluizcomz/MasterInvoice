using Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("invoices")]
    public class Invoice
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; } // Identificador único da nota fiscal

        [Required]
        [MaxLength(255)]
        [Display(Name = "Nome do pagador")]
        public required string PayerName { get; set; } // Nome do pagador

        [Required]
        [MaxLength(50)]
        [Display(Name = "Nº de identificação")]
        public required string IdentificationNumber { get; set; } // Número de identificação único

        [Required]
        [Display(Name = "Data de emissão")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime IssueDate { get; set; } // Data de emissão da nota fiscal

        [Display(Name = "Data de cobrança")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? BillingDate { get; set; } // Data de cobrança
        [MaxLength(255)]
        [Display(Name = "Doc. da nota fiscal")]

        public required string InvoiceDoc { get; set; } // Documento da nota fiscal

        [MaxLength(255)]
        [Display(Name = "Doc. do boleto bancário")]
        public required string BillDoc { get; set; } // Documento do boleto bancário

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; } // Valor da nota fiscal


        [ForeignKey("StatusId")]
        [Display(Name = "Status")]
        public StatusType StatusId { get; set; } // Identificador de status

        //public Status Status { get; set; } // Chave estrangeira para a tabela de status

    }

}
