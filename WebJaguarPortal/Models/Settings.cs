namespace WebJaguarPortal.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public string? SmtpAddress { get; set; }
        public string? SmtpFrom { get; set; }
        public string? SmtpUsername { get; set; }
        public string? SmtpPassword { get; set; }
        public bool SmtpUseSSL { get; set; }
        public int? SmtpPort { get; set; }
        public string JWTSigningKey { get; set; }
        public int EntropyLevelPassword { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
