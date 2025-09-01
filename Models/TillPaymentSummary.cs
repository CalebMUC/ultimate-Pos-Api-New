using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.Models
{
    public class TillPaymentSummary
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int TillId { get; set; }
        public Till Till { get; set; }

        [Required]
        public int PaymentMethodId { get; set; }
        public Payments PaymentMethod { get; set; }

        [Required]
        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal TotalAmount { get; set; }
    }

}
