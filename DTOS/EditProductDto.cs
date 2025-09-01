using System.ComponentModel.DataAnnotations;

namespace Ultimate_POS_Api.DTOS
{
    public class EditProductDto
    {
        public Guid ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public string ProductDescription { get; set; } = string.Empty;

        public int? Weight_Volume { get; set; }

        public Guid CategoryID { get; set; }

        public decimal BuyingPrice { get; set; }

        public decimal SellingPrice { get; set; }

        public int Quantity { get; set; }

        public Guid Supplier { get; set; }
    }
}
