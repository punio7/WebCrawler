using Microsoft.Owin;
using Owin;
using WebCrawler.ChatApp.Logic;

[assembly: OwinStartupAttribute(typeof(ChatApp.Startup))]
namespace ChatApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();
            WorkerManager.Instance.UnregisterAllWorkers();
        }
    }
}
