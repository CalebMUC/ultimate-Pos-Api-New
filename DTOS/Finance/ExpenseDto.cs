namespace Ultimate_POS_Api.DTOS.Finance
{
    public class ExpenseDto
    {
        public Guid expenseCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ReferenceNo { get; set; }
    }
}
