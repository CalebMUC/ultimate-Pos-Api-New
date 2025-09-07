namespace Ultimate_POS_Api.DTOS.Till
{
    public class AssignTillDto
    {
        public int TillId { get; set; }
        public Guid UserId { get; set; }
        public string AssignedBy { get; set; }
    }
}
