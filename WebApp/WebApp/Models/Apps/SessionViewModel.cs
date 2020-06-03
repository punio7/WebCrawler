using WebCrawler.WebApp.DbModel.Enums;

namespace WebCrawler.WebApp.WebApp.Models.Apps
{
    public class SessionViewModel
    {
        public long Id { get; set; }
        public string AppDisplayName { get; set; }
        public SessionState State { get; set; }
        public string StateDisplay
        {
            get
            {
                switch (State)
                {
                    case SessionState.NotStarted:
                        return "Nie wystartowała";
                    case SessionState.Active:
                        return "Aktywna";
                    case SessionState.Idle:
                        return "Nieaktywna";
                    case SessionState.Finished:
                        return "Zakończona";
                    default:
                        return State.ToString();
                }
            }
        }
        public string CreatorDisplayName { get; set; }
    }
}