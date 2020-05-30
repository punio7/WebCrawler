using WebCrawler.WebApp.DbModel.Enums;

namespace WebCrawler.WebApp.WebApp.Models.Apps
{
    public class SessionViewModel
    {
        public long Id { get; set; }
        public string AppDisplayName { get; set; }
        public SessionState State { get; set; }
        public string CreatorDisplayName { get; set; }
    }
}