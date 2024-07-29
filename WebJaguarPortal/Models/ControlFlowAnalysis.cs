using System.ComponentModel.DataAnnotations.Schema;

namespace WebJaguarPortal.Models
{
    public class ControlFlowAnalysis
    {
        public long Id { get; set; }

        public long ProjectId { get; set; }

        public virtual IEnumerable<ClassAnalysis> Classes { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        public StatusAnalysis Status { get; set; }

        public DateTime StartAnalysis { get; set; }
        public DateTime? EndAnalysis { get; set; }
        public string? MessageError { get; set; }
        public int TestsFail { get; set; }
        public int TestsPass { get; set; }
        public string Provider { get; set; }
        public string Repository { get; set; }
        public string PullRequestBase { get; set; }
        public string PullRequestBranch { get; set; }
        public string PullRequestNumber { get; set; }
    }

    public enum StatusAnalysis
    {
        InProgress = 0,
        Completed = 1,
        Error = 2,
    }
}
