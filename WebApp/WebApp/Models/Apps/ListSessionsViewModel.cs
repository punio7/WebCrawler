using System.Collections.Generic;
using System.Linq;

namespace WebCrawler.WebApp.WebApp.Models.Apps
{
    public class ListSessionsViewModel
    {
        public IEnumerable<SessionViewModel> Sessions { get; set; } = Enumerable.Empty<SessionViewModel>();
    }
}
