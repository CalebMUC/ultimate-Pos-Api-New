using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class Expense
    {
        [Key]
        [Required]
        public Guid expenseId { get; set; }
        [Required]
        [ForeignKey("expenseCategory")]
        public Guid expenseCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string ReferenceNo { get; set; }

        //navigation property
        public ExpenseCategory expenseCategory { get; set; }


    }
}
