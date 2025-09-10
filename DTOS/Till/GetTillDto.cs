using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.DTOS.Till
{
    public class GetTillDto
    {
        public int TillId { get; set; }
        public string? Name { get; set; }
        public string? CashierName { get; set; }
        public Guid? UserId { get; set; }
        public string? Description { get; set; }
        public decimal? OpeningAmount { get; set; }
        public decimal? ClosingAmount { get; set; }
        public decimal? CurrentAmount { get; set; }
        public decimal? ExpectedAmount { get; set; }
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
    }
}
