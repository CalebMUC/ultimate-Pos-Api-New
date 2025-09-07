namespace Ultimate_POS_Api.DTOS.Till
{
    public class TillClosureDto
    {
        public int TillId { get; set; }
        public decimal ClosingAmount { get; set; }
        public decimal Variance { get; set; }
        public string ClosedBy { get; set; }
    }
}
