using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.WorkerApp.Logic.Config
{
    public class AppsConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("Apps", IsDefaultCollection = false)]
        [ConfigurationCollection(typeof(AppsCollection),
            AddItemName = "add",
            ClearItemsName = "clear",
            RemoveItemName = "remove")]
        public AppsCollection Apps
        {
            get
            {
                return (AppsCollection)base["Apps"];
            }
        }
    }
}
