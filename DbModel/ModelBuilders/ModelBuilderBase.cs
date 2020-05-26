using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace WebCrawler.WebApp.DbModel.ModelBuilders
{
    internal abstract class ModelBuilderBase<TEntity> : IModelBuilder
        where TEntity: class
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TEntity>(Build);
        }

        protected abstract void Build(EntityTypeBuilder<TEntity> builder);
    }
}
