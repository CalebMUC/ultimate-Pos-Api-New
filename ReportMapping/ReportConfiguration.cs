namespace Ultimate_POS_Api.ReportMapping
{
    public static class ReportConfiguration
    {
        public static readonly Dictionary<string, ReportDefinition> ReportDefinitions = new Dictionary<string, ReportDefinition>
        {
            {
                "StockAlertReport", new ReportDefinition
                {
                    Endpoint = "StockReports",
                    Parameters = new List<string> { "lowStockThreshold","format" }
                }
            },
            {
                "SalesReport", new ReportDefinition
                {
                    Endpoint = "SalesReports",
                    Parameters = new List<string> { "StartDate", "EndDate" }
                }
            },
            {
                "PurchaseReport", new ReportDefinition
                {
                    Endpoint = "PurchaseReports",
                    Parameters = new List<string> { "StartDate", "EndDate" }
                }
            },
            {
                "CustomerReport", new ReportDefinition
                {
                    Endpoint = "CustomerReports",
                    Parameters = new List<string> { "CustomerId" }
                }
            },
            {
                "SupplierReport", new ReportDefinition
                {
                    Endpoint = "SupplierReports",
                    Parameters = new List<string> { "SupplierId" }
                }
            }
        };
    }
}
