namespace Ultimate_POS_Api.DTOS.Till
{
    public class CloseTillDto
    {
        public int TillId { get; set; }
        public decimal ClosingAmount { get; set; }

        public string ClosedBy { get; set; } = string.Empty;
    }
}
