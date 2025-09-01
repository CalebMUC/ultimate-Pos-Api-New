namespace Ultimate_POS_Api.DTOS
{
    public class TransactionProductsDTO
    {
        public string ProductID { get; set; } = string.Empty; // VARCHAR in PostgreSQL
        public string ProductName { get; set; } = string.Empty; // VARCHAR in PostgreSQL
        public int Quantity { get; set; } // INTEGER in PostgreSQL
        public double Price { get; set; } // DOUBLE PRECISION in PostgreSQL
        public double Discount { get; set; } // DOUBLE PRECISION in PostgreSQL
        public double ValueAddedTax { get; set; } // DOUBLE PRECISION in PostgreSQL
    }
}
