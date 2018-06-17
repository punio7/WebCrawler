using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.WorkerApp.Logic.Config
{
    public class AppConfig : ConfigurationElement
    {
        public AppConfig() { }

        public AppConfig(string name, string exePath)
        {
            Name = name;
            ExePath = exePath;
        }

        [ConfigurationProperty("Name", DefaultValue = "", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("ExePath", IsRequired = true, IsKey = false)]
        public string ExePath
        {
            get { return (string)this["ExePath"]; }
            set { this["ExePath"] = value; }
        }
    }
}
