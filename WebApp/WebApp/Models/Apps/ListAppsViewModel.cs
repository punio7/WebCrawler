using System.Collections.Generic;
using System.Linq;
using WebCrawler.WebApp.DbModel.Models;

namespace WebCrawler.WebApp.WebApp.Models.Apps
{
    public class ListAppsViewModel
    {
        public IEnumerable<AppDefinition> Apps { get; set; } = Enumerable.Empty<AppDefinition>();
    }
}
