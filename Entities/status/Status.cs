using Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    [Table("status")]
    public class Status
    {
        [Key]
        public StatusType Id { get; set; } // Identificador único do status

        [Required]
        [MaxLength(255)]
        public required string Description { get; set; } // Descrição do status
    }
}
