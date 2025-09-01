using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class Till
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TillId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "numeric(18,2)")]
        public decimal OpeningAmount { get; set; }
        [Required]
        [Column(TypeName = "numeric(18,2)")]
        public decimal ClosingAmount { get; set; }
        [Required]
        [Column(TypeName = "numeric(18,2)")]
        public decimal CurrentAmount { get; set; }
        [Required]
        [Column(TypeName = "numeric(18,2)")]
        public decimal ExpectedAmount { get; set; }
        [Required]
        [Column(TypeName = "numeric(18,2)")]
        public decimal Variance { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UpdatesBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string SupervisedBy { get; set; }
        public DateTime SupervisedOn { get; set; }

        // Navigation property (Cashier / User operating this till)
        public User User { get; set; }
    }
}
