namespace Ultimate_POS_Api.DTOS.Finance
{
    public class EditExpenseCategoryDto
    {
        public Guid expenseCategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string UpdatedBy { get; set; }
    }
}
