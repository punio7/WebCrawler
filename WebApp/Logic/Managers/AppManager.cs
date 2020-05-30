using System.Collections.Generic;
using System.Linq;
using WebCrawler.WebApp.DbModel;
using WebCrawler.WebApp.DbModel.Models;

namespace WebCrawler.WebApp.Logic
{
    public class AppManager
    {
        private readonly WebCrawlerDbContext dbContext;

        public AppManager(WebCrawlerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<AppDefinition> GetAppDefinitions()
        {
            return dbContext.AppDefinitions.OrderBy(app => app.Name);
        }
    }
}
