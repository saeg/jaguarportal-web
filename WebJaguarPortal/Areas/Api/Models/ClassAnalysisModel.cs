namespace WebJaguarPortal.Areas.Api.Models
{
    public class ClassAnalysisModel
    {
        public string FullName { get; set; }

        public byte[]? Code { get; set; }

        public ICollection<LineAnalysisModel> Lines { get; set; }
    }
}
