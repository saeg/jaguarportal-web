namespace WebJaguarPortal.Models
{
    public class FileAnalysis
    {
        public long Id { get; set; }        
        public string Hash { get; set; }
        public byte[] Data { get; set; }
    }
}
