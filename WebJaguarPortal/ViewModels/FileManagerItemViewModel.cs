namespace WebJaguarPortal.ViewModels
{
    public class FileManagerItemViewModel
    {
        public bool IsPath { get; set; }
        public bool IsFile { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; }
        public string? Fullname { get; set; }
        public string? Extension { get; set; }
        public long? IdClass { get; set; }
        public string? Icon
        {
            get
            {
                return Extension switch
                {
                    ".java" => "java",
                    _ => "code",
                };
            }
        }
        public List<FileManagerItemViewModel>? FilesAndPaths { get; set; }
    }
}
