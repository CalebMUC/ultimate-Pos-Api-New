using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Ultimate_POS_Api.Models
{
    // Supplier entity
    public class Supplier
    {
        [Key]
        public Guid SupplierId { get; set; } // Use GUID (UUID) for SupplierId

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string SupplierName { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string SupplierType { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string Industry { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string KRAPIN { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string BusinessLicenseNumber { get; set; } = string.Empty;

        [Required]
        public bool SupplierStatus { get; set; }

        public string Remarks { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string CreatedBy { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "VARCHAR(255)")]
        public string UpdatedBy { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedOn { get; set; } // Use DateTime for CreatedOn

        [Required]
        public DateTime UpdatedOn { get; set; }

        // Contact info
        [Column(TypeName = "VARCHAR(255)")]
        public string Email { get; set; } = string.Empty;

        [Column(TypeName = "VARCHAR(255)")]
        public string Phone { get; set; } = string.Empty;

        // Address info details
        [Column(TypeName = "VARCHAR(255)")]
        public string LocationName { get; set; } = string.Empty;

        [Column(TypeName = "VARCHAR(255)")]
        public string Town { get; set; } = string.Empty;

        [Column(TypeName = "VARCHAR(255)")]
        public string Postal { get; set; } = string.Empty;

        // Contract details
        [Column(TypeName = "VARCHAR(255)")]
        public string ContractStartDate { get; set; } = string.Empty;

        [Column(TypeName = "VARCHAR(255)")]
        public string ContractEndDate { get; set; } = string.Empty;

        [Column(TypeName = "VARCHAR(255)")]
        public string ContractTerms { get; set; } = string.Empty;

        [Required]
        public bool Status { get; set; } // Fixed status to boolean type

        [Column(TypeName = "VARCHAR(255)")]
        public string Category { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string? UnitMeasure { get; set; }

        // Bank details
        [Column(TypeName = "VARCHAR(255)")]
        public string BankName { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string Bank_AccountNumber { get; set; }

        // Mpesa details
        [Column(TypeName = "VARCHAR(255)")]
        public string Till { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string Pochi { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string Paybill_BusinessNumber { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string Paybill_Account { get; set; }

        // Product details
        [JsonIgnore]
        public ICollection<Products> Products { get; set; }
    }
}
