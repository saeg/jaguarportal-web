namespace WebJaguarPortal.Areas.Api.Models
{
    public class AnalysisControlFlowModel
    {
        public string ProjectKey { get; set; }
        public int TestsFail { get; set; }
        public int TestsPass { get; set; }
        public string Provider { get; set; }
        public string Repository { get; set; }
        public string? PullRequestBase { get; set; }
        public string? PullRequestBranch { get; set; }
        public string? PullRequestNumber { get; set; }
    }
}
