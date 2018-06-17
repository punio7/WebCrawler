using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.WorkerApp.Logic.Config;

namespace WebCrawler.WorkerApp.Logic
{
    public static class Configuration
    {
        public static string GetAppPath(string appName)
        {
            var appsSection = ConfigurationManager.GetSection("AppsSection") as AppsConfigurationSection;
            foreach (AppConfig app in appsSection.Apps)
            {
                if (app.Name == appName)
                {
                    return app.ExePath;
                }
            }
            return String.Empty;
        }

    }
}
