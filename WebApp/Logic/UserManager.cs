using System.Linq;
using WebCrawler.WebApp.DbModel;
using WebCrawler.WebApp.DbModel.Models;

namespace WebCrawler.WebApp.Logic
{
    public class UserManager
    {
        private readonly WebCrawlerDbContext dbContext;

        public UserManager(WebCrawlerDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public ApplicationUser GetUserById(string id)
        {
            return dbContext.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}