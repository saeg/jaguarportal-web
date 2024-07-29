namespace WebJaguarPortal.Models
{
    public class RenewPassword
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Username { get; set; }
    }
}
