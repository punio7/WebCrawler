using Microsoft.AspNetCore.Identity;

namespace WebCrawler.WebApp.DbModel.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}
