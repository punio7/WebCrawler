using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebCrawler.WebApp.DbModel.Models;

namespace WebCrawler.WebApp.DbModel.ModelBuilders
{
    class ProcessSessionBuilder : ModelBuilderBase<ProcessSession>
    {
        protected override void Build(EntityTypeBuilder<ProcessSession> builder)
        {
            builder.HasKey(ps => ps.Id);

            builder.HasIndex(ps => ps.WorkerConnectionId);
            builder.HasIndex(ps => ps.State);

            builder.Ignore(ps => ps.GroupName);

            builder.Property(wc => wc.CreatorId)
                .IsRequired();
            builder.Property(wc => wc.AppName)
                .IsRequired();
        }
    }
}
