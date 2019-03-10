using System.Web;
using System.Web.Mvc;
using WebCrawler.ChatApp.Filters;

namespace ChatApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LoggingActionFilter());
        }
    }
}
