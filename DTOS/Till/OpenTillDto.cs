namespace Ultimate_POS_Api.DTOS.Till
{
    public class OpenTillDto
    {
        public int TillId { get; set; }
        public decimal OpeningAmount { get; set; }

        public decimal ExpectedAmount { get; set; }

        public string OpenedBy { get; set; } = string.Empty;
    }
}
