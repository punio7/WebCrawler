using System.Collections.Generic;
using System.Linq;

namespace WebCrawler.WebApp.WebApp.Models.Apps
{
    public class ListAppsViewModel
    {
        public IEnumerable<AppViewModel> Apps { get; set; } = Enumerable.Empty<AppViewModel>();
    }
}
