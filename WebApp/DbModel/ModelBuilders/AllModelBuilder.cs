using System;
using System.Collections.Generic;
using System.Linq;
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
            Type iModelBuilderType = typeof(IModelBuilder);

            var types = typeof(AllModelBuilder).Assembly.GetTypes();
            var classes = types.Where(t => t.IsClass && !t.IsAbstract);
            var iModelBuilders = classes.Where(t => iModelBuilderType.IsAssignableFrom(t));
            return iModelBuilders.Select(t => (IModelBuilder)Activator.CreateInstance(t));
        }
    }
}
