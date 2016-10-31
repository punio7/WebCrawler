using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebCrawler.HubModel.ServerModels;

namespace ChatApp.HubModel
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base(ConnectionString, throwIfV1Schema: false)
        {
        }

        private static string ConnectionString
        {
            get
            {
                return "AzureConnection";
            }
        }

        public DbSet<WorkerConnection> WorkerConnections { get; set; }
        public DbSet<ProcessSession> ProcessSessions { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}