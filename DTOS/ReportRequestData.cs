using System.Collections.Generic;

namespace Ultimate_POS_Api.DTOS
{
    public class ReportRequestData
    {
        public string ReportType { get; set; } // VARCHAR in PostgreSQL
        public string Format { get; set; } // VARCHAR in PostgreSQL

        public Dictionary<string, object> parameters { get; set; } // This could be stored as JSON in PostgreSQL
    }
}
