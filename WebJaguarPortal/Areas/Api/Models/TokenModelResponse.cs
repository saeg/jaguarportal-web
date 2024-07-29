namespace WebJaguarPortal.Areas.Api.Models
{
    public class TokenModelResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string username { get; set; }
    }
}
