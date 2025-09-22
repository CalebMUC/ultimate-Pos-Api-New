namespace Ultimate_POS_Api.DTOS.Finance
{
    public class GetExpenseCategory
    {

        public Guid expensecategoryid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string createdby { get; set; }
        public DateTime createdon { get; set; }
        public string? updatedby { get; set; }
        public DateTime? updatedon { get; set; }
    }
}
