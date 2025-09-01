namespace Ultimate_POS_Api.DTOS
{
    public class UserInfo
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
        public string DeviceInfo { get; set; } = string.Empty;
        public string IpAddress { get; set; } = string.Empty;

    }

    public class UserLogins
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
    }

}
