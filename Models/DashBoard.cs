using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class DashBoard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int NoTransactions { get; set; }

        [Required]
        public int AvailableProducts { get; set; }

        [Required]
        public int TotalSales { get; set; }

        [Required]
        public int TotalCash { get; set; }
    }
}
