using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class Till
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TillId { get; set; }

        public string? Name { get; set; }
        public Guid? UserId { get; set; }
        public string? Description { get; set; }

        [Column(TypeName = "numeric(18,2)")]
        public decimal? OpeningAmount { get; set; }

        [Column(TypeName = "numeric(18,2)")]
        public decimal? ClosingAmount { get; set; }

        [Column(TypeName = "numeric(18,2)")]
        public decimal? CurrentAmount { get; set; }

        [Column(TypeName = "numeric(18,2)")]
        public decimal? ExpectedAmount { get; set; }

        [Column(TypeName = "numeric(18,2)")]
        public decimal? Variance { get; set; }

        public string? Status { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string? OpenedBy { get; set; }
        public DateTime? OpenedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? SupervisedBy { get; set; }
        public DateTime? SupervisedOn { get; set; }

        public bool IsDeleted { get; set; } = false;


        // Navigation property (Cashier / User operating this till)
        public User? User { get; set; }
    }
}
