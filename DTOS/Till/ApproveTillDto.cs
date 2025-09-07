namespace Ultimate_POS_Api.DTOS.Till
{
    public class ApproveTillDto
    {
        public int TillId { get; set; }
        public string Supervisor { get; set; }
        public bool Approved { get; set; }
    }
}
