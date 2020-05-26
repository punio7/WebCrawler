using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebCrawler.WebApp.DbModel.Models;

namespace WebCrawler.WebApp.DbModel.ModelBuilders
{
    class WorkerConnectionBuilder : ModelBuilderBase<WorkerConnection>
    {
        protected override void Build(EntityTypeBuilder<WorkerConnection> builder)
        {
            builder.HasKey(wc => wc.Id);
            builder.HasIndex(wc => wc.WorkerName)
                .IsUnique();

            builder.Property(wc => wc.WorkerName)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
