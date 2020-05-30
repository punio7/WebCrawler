using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebCrawler.WebApp.DbModel.Models;

namespace WebCrawler.WebApp.DbModel.ModelBuilders
{
    class AppDefinitionBuilder : ModelBuilderBase<AppDefinition>
    {
        protected override void Build(EntityTypeBuilder<AppDefinition> builder)
        {
            builder.HasKey(app => app.Id);

            builder.HasIndex(app => app.Name)
                .IsUnique();

            builder.Property(app => app.Name)
                .IsRequired();
            builder.Property(app => app.DisplayName)
                .IsRequired();
        }
    }
}
