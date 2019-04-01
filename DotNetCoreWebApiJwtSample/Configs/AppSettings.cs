namespace DotNetCoreWebApiJwtSample.Configs
{
    public class AppSettings
    {
        public JwtConfigurableOptions JwtSetting { get; set; }
    }

    public class JwtConfigurableOptions
    {
        public string JwtKey { get; set; }
        public string JwtIssuer { get; set; }
        public string JwtAudience { get; set; }
        public int JwtExpireDays { get; set; }
    }
}
