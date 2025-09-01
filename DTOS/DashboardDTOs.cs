using System;
using System.Collections.Generic; // Import for IList
using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS
{
    public class DashboardDTOs
    {
        [Required]
        public int NoTransactions { get; set; }  // Changed to PascalCase for consistency

        public int AvailableProducts { get; set; }  // Changed to PascalCase for consistency

        public int TotalSales { get; set; }  // Changed to PascalCase for consistency

        public int TotalCash { get; set; }  // Changed to PascalCase for consistency
    }

    public class DashboardlistDTOs
    {
        [Required]
        public IList<DashboardDTOs> Dashboard { get; set; } = new List<DashboardDTOs>();  // Changed to PascalCase for consistency
    }
}
