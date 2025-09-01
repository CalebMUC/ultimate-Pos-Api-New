using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.Models
{
    public class TransactionProducts
    {
        [Key]
        public Guid TransactionProductID { get; set; } = Guid.NewGuid();

        [ForeignKey("Transactions")]
        public Guid TransactionID { get; set; }
        public Transactions Transaction { get; set; }

        [ForeignKey("Products")]
        public Guid ProductID { get; set; }
        public Products Product { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "DECIMAL(18, 2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "DECIMAL(18, 2)")]
        public decimal Discount { get; set; }

        [Column(TypeName = "DECIMAL(18,2)")]
        public decimal SubTotal { get; set; }
    }
}
