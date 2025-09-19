using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS.Transactions
{
    public class GetTransactionsDto
    {
        public Guid TransactionId { get; set; } = Guid.NewGuid();

        public string InvoiceNumber { get; set; } // e.g. POS receipt number

        public decimal TotalAmount { get; set; } // Total sale amount

        public decimal Discount { get; set; } = 0;

        public decimal Tax { get; set; } = 0;

        public decimal NetAmount { get; set; } // (Total - Discount + Tax)


        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public string Cashier { get; set; } // User handling the transaction
        public string TillName { get; set; }
    }
}
