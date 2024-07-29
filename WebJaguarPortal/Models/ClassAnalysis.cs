using System.ComponentModel.DataAnnotations.Schema;

namespace WebJaguarPortal.Models
{
    public class ClassAnalysis
    {
        public long Id { get; set; }
        public long AnalysisId { get; set; }

        public string FullName { get; set; }

        public long? FileAnalyzeId { get; set; }

        [ForeignKey("FileAnalyzeId")]
        public virtual FileAnalysis? Code { get; set; }

        [ForeignKey("AnalysisId")]
        public virtual ControlFlowAnalysis Analyze { get; set; }

        public virtual ICollection<LineAnalysis> Lines { get; set; }
    }
}
