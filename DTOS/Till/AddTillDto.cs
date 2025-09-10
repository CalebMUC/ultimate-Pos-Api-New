using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS.Till
{
    public class AddTillDto
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
    
    }
}
