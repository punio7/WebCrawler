using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebCrawler.WebApp.DbModel.ModelBuilders
{
    internal static class AllModelBuilder
    {
        internal static void BuildAllModels(ModelBuilder modelBuilder)
        {
            IEnumerable<IModelBuilder> modelBuilders = GetAllModelBuilders();

            foreach (var builder in modelBuilders)
            {
                builder.Build(modelBuilder);
            }
        }

        private static IEnumerable<IModelBuilder> GetAllModelBuilders()
        {
            return new List<IModelBuilder>()
            {
                new ApplicationUserBuilder(),
                new ProcessSessionBuilder(),
                new WorkerConnectionBuilder(),
            };
        }
    }
}
