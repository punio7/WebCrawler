using WebCrawler.WebApp.DbModel.Enums;

namespace WebCrawler.WebApp.WebApp.Models.Apps
{
    public class SessionViewModel
    {
        public string AppName { get; internal set; }
        public SessionState State { get; internal set; }
        public string CreatorDisplayName { get; internal set; }
    }
}