using MasterInvoice.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterInvoice.Models.status
{
    [Table("status")]
    public class Status
    {
        [Key]
        public StatusType Id { get; set; } // Identificador único do status

        [Required]
        [MaxLength(255)]
        public string Description { get; set; } // Descrição do status
    }
}
