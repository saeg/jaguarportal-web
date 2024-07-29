namespace WebJaguarPortal.Areas.Api.Models
{
    public class TokenModelRequest
    {
        public string grant_type { get; set; }
        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
}
