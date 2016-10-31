using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WebCrawler.Common.Enums;

namespace WebCrawler.WorkerApp.ViewModel
{
    public class ProcessViewModel : ViewModelBase
    {
        public string Name
        {
            get
            {
                return ProcessInstance.Name;
            }
            set
            {
                ProcessInstance.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public int Pid
        {
            get
            {
                return ProcessInstance.Pid;
            }
            set
            {
                ProcessInstance.Pid = value;
                RaisePropertyChanged(nameof(Pid));
            }
        }

        public ProcessStatus Status
        {
            get
            {
                return ProcessInstance.Status;
            }
            set
            {
                ProcessInstance.Status = value;
                RaisePropertyChanged(nameof(Status));
            }
        }

        public string UserName
        {
            get
            {
                return ProcessInstance.User;
            }

            set
            {
                ProcessInstance.User = value;
                RaisePropertyChanged(nameof(UserName));
            }
        }

        public long SessionId
        {
            get
            {
                return ProcessInstance.SessionId;
            }

            set
            {
                ProcessInstance.SessionId = value;
                RaisePropertyChanged(nameof(SessionId));
            }
        }

        public Common.Model.ProcessInstance ProcessInstance { get; set; } = new Common.Model.ProcessInstance();

        public override void Cleanup()
        {
            ProcessInstance.Dispose();
            base.Cleanup();
        }
    }
}
