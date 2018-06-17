using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawler.WorkerApp.Logic.Config
{
    public class AppsCollection : ConfigurationElementCollection
    {
        public AppsCollection() : base()
        {
        }

        public AppConfig this[int index]
        {
            get { return (AppConfig)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(AppConfig serviceConfig)
        {
            BaseAdd(serviceConfig);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AppConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AppConfig)element).Name;
        }

        public void Remove(AppConfig serviceConfig)
        {
            BaseRemove(serviceConfig.Name);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }
    }
}
