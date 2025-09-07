using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.DTOS.Till
{
    public class UpdateTillDto
    {
        public int TillId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
