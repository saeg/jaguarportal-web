using WebJaguarPortal.Models;

namespace WebJaguarPortal.ViewModels
{
    public class CodeViewModel
    {
        public string ClassName { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
        public string Path { get; internal set; }
        public ICollection<LineAnalysis> Lines { get; set; }
    }
}
