namespace Ultimate_POS_Api.DTOS
{
    public class LoginResponseStatus
    {
        public int Status { get; set; }
        public string StatusMessage { get; set; } = string.Empty;

        public string Token { get; set; }

        public string Name { get; set; }

        public string UserRole { get; set; }
        public Guid SessionId { get; set; }
    }
}
