using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.Models
{
    public class Supplies
    {
        [Key]
        public Guid SupplyId { get; set; } 

        [Column(TypeName = "VARCHAR(255)")]
        public Guid SupplierId { get; set; } 

        // Use DateTime for the supply date instead of string
        public DateTime SupplyDate { get; set; }

        [ForeignKey("Categories")]
        public Guid CategoryID { get; set; } 

        // Navigation property for Categories
        public Categories Categories { get; set; }

        [ForeignKey("Products")]
        public Guid ProductID { get; set; } 

        // Navigation property for Products
        public Products Products { get; set; }

        public int Quantity { get; set; }


        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "VARCHAR(255)")]
        public string CreatedBy { get; set; }


        [Column(TypeName = "VARCHAR(255)")]
        public string UpdatedBy { get; set; }





    }
}
