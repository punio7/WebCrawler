using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;

namespace WebCrawler.WorkerApp.Common
{
    public class ObservableStringBuilder : ObservableObject
    {
        private StringBuilder stringBuilder;
        public ObservableStringBuilder()
        {
            stringBuilder = new StringBuilder();
        }

        public string StringValue
        {
            get
            {
                return stringBuilder.ToString();
            }
        }

        public ObservableStringBuilder Append(string value)
        {
            stringBuilder.Append(value);
            RaisePropertyChanged(nameof(StringValue));
            return this;
        }

        public ObservableStringBuilder AppendLine(string value)
        {
            stringBuilder.AppendLine(value);
            RaisePropertyChanged(nameof(StringValue));
            return this;
        }

        public ObservableStringBuilder Clear()
        {
            stringBuilder.Clear();
            RaisePropertyChanged(nameof(StringValue));
            return this;
        }
    }
}
