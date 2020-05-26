using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Areas.Identity.Services;
using WebCrawler.WebApp.Logic;

namespace WebCrawler.WebApp.WebApp
{
    public static partial class RegisterDependenciesExtension
    {
        public static IServiceCollection RegisterDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, EmailSender>();

            services.AddScoped<SessionManager>();
            services.AddScoped<UserManager>();
            services.AddScoped<WorkerManager>();

            return services;
        }
    }
}
