using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.WorkerApp.Logic.Managers
{
    public class ApplicationsManager
    {
        public string GetApplicationExecutablePath(string appName)
        {
            return Configuration.GetAppPath(appName);
        }
    }
}
