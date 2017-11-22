using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChatApp.HubModel;
using WebCrawler.HubModel.ServerModels;

namespace WebCrawler.ChatApp.Logic
{
    public class UserManager
    {
        public ApplicationUser GetUserById(string id)
        {
            using (ApplicationDbContext dbContext = ApplicationDbContext.Create())
            {
                return dbContext.Users.FirstOrDefault(u => u.Id == id);
            }
        }
    }
}