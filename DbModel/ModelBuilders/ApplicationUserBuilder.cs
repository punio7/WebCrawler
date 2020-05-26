using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebCrawler.WebApp.DbModel.Models;

namespace WebCrawler.WebApp.DbModel.ModelBuilders
{
    class ApplicationUserBuilder : ModelBuilderBase<ApplicationUser>
    {
        protected override void Build(EntityTypeBuilder<ApplicationUser> builder)
        {
        }
    }
}
