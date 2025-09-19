using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.Models
{
    public class ExpenseCategory
    {
        [Key]
        [Required]
        public Guid expensecategoryid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool isactive { get; set; }
        public string createdby { get; set; }
        public DateTime createdon { get; set; }
        public string? updatedby { get; set; }
        public DateTime? updatedon { get; set; }

    }
}
