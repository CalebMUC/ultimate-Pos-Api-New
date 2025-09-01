using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ultimate_POS_Api.DTOS
{
    public class SuppliersDTO
    {
        [Required]
        public Guid SupplierId { get; set; }

        [Required]
        public string SupplierName { get; set; } = string.Empty; 

        [Required]
        public string SupplierType { get; set; } = string.Empty; 

        [Required]
        public string Industry { get; set; } = string.Empty; 

        [Required]
        public string KRAPIN { get; set; } = string.Empty; 

        [Required]
        public string BusinessLicenseNumber { get; set; } = string.Empty;

        [Required]
        public bool SupplierStatus { get; set; } 

        public string Remarks { get; set; } = string.Empty; 


        // contact details
        public string Email { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty; 

        // address details
        public string LocationName { get; set; } = string.Empty; 

        public string Town { get; set; } = string.Empty; 

        public string Postal { get; set; } = string.Empty;

        // contract details
        public string ContractStartDate { get; set; } = string.Empty; 
        public string ContractEndDate { get; set; } = string.Empty; 

        public string ContractTerms { get; set; } = string.Empty; 

        public bool ContractStatus { get; set; }

        // product details
        public string? Category { get; set; }
        public string? UnitMeasure { get; set; } 

        // Bank details
        public string? BankName { get; set; } 
        public string? Bank_AccountNumber { get; set; } 

        // mpesa details
        public string? Till { get; set; } 
        public string? Pochi { get; set; } 

        public string? Paybill_BusinessNumber { get; set; } 
        public string? Paybill_Account { get; set; } 
    }

    public class SuppliersDetailsDTO
    {
        [Required]
        public IList<SuppliersDTO> Supplier { get; set; } = new List<SuppliersDTO>();
    }
}
