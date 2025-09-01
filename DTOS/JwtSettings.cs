namespace Ultimate_POS_Api.DTOS
{
    public class JwtSettings
    { 

        public string Key { get; set; }
        public string Secret { get; set; }  // JWT secret key for signing tokens

        public string Issuer { get; set; }  // The issuer of the token (e.g., your application name)

        public string Audience { get; set; }  // The intended audience of the token (e.g., your API or service)

        public int ExpirationInMinutes { get; set; }  // Expiration time for the token in minutes
    }
}
