namespace Mocker.Web.Models.Settings
{
    public class GitHubSetting
    {
        public long RepositoryID { get; set; }
        public string Branch { get; set; }
        public string HttpMethodsFolderPath { get; set; }
        public string User { get; set; }
        public string Pass { get; set; }
        public string FilePath { get; set; }
    }
}
