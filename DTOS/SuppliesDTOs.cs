using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS
{
    public class SuppliesDTOs
    {
        [Key]
        public string SupplyId { get; set; } = string.Empty; // VARCHAR in PostgreSQL

        [Required]
        public Guid SupplierId { get; set; } //= string.Empty; // VARCHAR in PostgreSQL

        [Required]
        public DateTime SuppllyDate { get; set; } //= string.Empty; // DATE or VARCHAR in PostgreSQL (preferably DATE)

        [Required]
        public Guid CategoryID { get; set; } // VARCHAR in PostgreSQL

        [Required]
        public Guid ProductID { get; set; } // VARCHAR in PostgreSQL

        [Required]
        public int Quantity { get; set; } // INTEGER in PostgreSQL
    }

    public class SuppliesDetailsDTO
    {
        [Required]
        public IList<SuppliesDTOs> Supplies { get; set; } = new List<SuppliesDTOs>();
    }
}
