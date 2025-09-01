namespace Ultimate_POS_Api.DTOS
{
    public class ResponseStatus
    {
        public int Status { get; set; }
        public string StatusMessage { get; set; } = string.Empty;

        // public object? Data { get; set; } 
    }
      public class ResponseAuth
    {
        public int Status { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
         public string Token { get; set; } = string.Empty;
    }

    public class ResponseStatusData
    {
        public bool Status { get; set; }
        public string StatusMessage { get; set; } = string.Empty;
        public object? Data { get; set; } // Optional field
    }

}
