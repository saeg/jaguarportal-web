using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace WebJaguarPortal.ViewModels
{
    public class AnalyzeGridItemViewModel
    {
        public long Id { get; set; }
        public string ProjectName { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public int TestsFail { get; set; }
        public int TestsPass { get; set; }
        public string Provider { get; set; }
        public string Repository { get; set; }
        public string PullRequestBase { get; set; }
        public string PullRequestBranch { get; set; }
        public string PullRequestNumber { get; set; }
        public string PullRequestFromTo => $"from '{PullRequestBranch}' to '{PullRequestBase}'";

        public string PullRequestUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(PullRequestNumber))
                {
                    switch (Provider.ToLowerInvariant())
                    {
                        case "github":
                            return $"https://github.com/{Repository}/pull/{PullRequestNumber}";
                        default:
                            return string.Empty;
                    }
                }
                else
                    return string.Empty;
            }
        }
        public string RepositoryUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(Repository))
                {
                    switch (Provider.ToLowerInvariant())
                    {
                        case "github":
                            return $"https://github.com/{Repository}";
                        default:
                            return string.Empty;
                    }
                }
                else
                    return string.Empty;
            }
        }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class AnalyzeDetailViewModel
    {
        public IEnumerable<FileManagerItemViewModel>? FilesAndPaths { get; set; }
        public IEnumerable<LineAnalysisViewModel>? Susp { get; set; }
    }

    public class AnalyzeDeleteViewModel
    {
        public long Id { get; set; }
        public string? ProjectName { get; set; }
        public string? Repository { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
