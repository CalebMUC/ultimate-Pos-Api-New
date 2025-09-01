namespace Ultimate_POS_Api.Models
{
    public class Reports
    {
        public class JasperExportData
        {
            public string ReportType { get; set; }
            public string Format { get; set; }
            public Dictionary<string,object> Parameters { get; set; }
        }
    }
}
