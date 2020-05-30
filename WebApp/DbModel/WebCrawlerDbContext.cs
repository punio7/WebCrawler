using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebCrawler.WebApp.DbModel.ModelBuilders;
using WebCrawler.WebApp.DbModel.Models;

namespace WebCrawler.WebApp.DbModel
{
    public class WebCrawlerDbContext : IdentityDbContext<ApplicationUser>
    {
        public WebCrawlerDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            AllModelBuilder.BuildAllModels(modelBuilder);
        }

        public DbSet<WorkerConnection> WorkerConnections { get; set; }
        public DbSet<ProcessSession> ProcessSessions { get; set; }
    }
}
