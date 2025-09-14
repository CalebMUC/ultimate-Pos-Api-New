using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.Models
{
    public class IncomeSources
    {
        [Key]
        [Required]
        public Guid incomeSourceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
