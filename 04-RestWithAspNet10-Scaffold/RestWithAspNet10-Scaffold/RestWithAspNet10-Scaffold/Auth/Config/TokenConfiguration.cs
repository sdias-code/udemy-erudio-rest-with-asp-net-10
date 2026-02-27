namespace RestWithAspNet10_Scaffold.Auth.Config
{
    public class TokenConfiguration
    {
        public TokenConfiguration() { }
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public int Minutes { get; set; }
        public int DaysToExpiry { get; set; }

    }
}
