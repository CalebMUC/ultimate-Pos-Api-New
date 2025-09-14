using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.Models
{
    public class Incomes
    {
        [Key]
        [Required]
        public Guid incomeId { get; set; }
        public Guid incomeSourceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime IncomeDate { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string ReferenceNo { get; set; }
    }
}
