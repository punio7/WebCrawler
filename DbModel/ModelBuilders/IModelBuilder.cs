using Microsoft.EntityFrameworkCore;

namespace WebCrawler.WebApp.DbModel.ModelBuilders
{
    internal interface IModelBuilder
    {
        void Build(ModelBuilder modelBuilder);
    }
}