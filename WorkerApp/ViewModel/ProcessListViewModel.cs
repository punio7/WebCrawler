using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WebCrawler.WorkerApp.Common.Messages;

namespace WebCrawler.WorkerApp.ViewModel
{
    public class ProcessListViewModel : ViewModelBase
    {
        /// <summary>
        /// The <see cref="ProcessList" /> property's name.
        /// </summary>

        private ObservableCollection<ProcessViewModel> _processList = new ObservableCollection<ProcessViewModel>();

        public ObservableCollection<ProcessViewModel> ProcessList
        {
            get
            {
                return _processList;
            }

            set
            {
                if (_processList == value)
                {
                    return;
                }

                _processList = value;
                RaisePropertyChanged(nameof(ProcessList));
            }
        }

        private ProcessViewModel _selectedProcess = null;
        public ProcessViewModel SelectedProcess
        {
            get
            {
                return _selectedProcess;
            }
            set
            {
                if (_selectedProcess == value)
                {
                    return;
                }
                _selectedProcess = value;
                RaisePropertyChanged(nameof(SelectedProcess));
            }
        }

        public void AddProcess(ProcessViewModel process)
        {
            ProcessList.Add(process);
        }

        public void RemoveProcess(ProcessViewModel process)
        {
            ProcessList.Remove(process);
        }

        public override void Cleanup()
        {
            foreach (var process in ProcessList)
            {
                process.Cleanup();
            }
            base.Cleanup();
        }

        #region ProcessSelectionChange Command

        private RelayCommand _processSelectionChange;

        public RelayCommand ProcessSelectionChange
        {
            get
            {
                return _processSelectionChange
                    ?? (_processSelectionChange = new RelayCommand(
                    () =>
                    {
                        MessengerInstance.Send(new ProcessSelectionChangeMessage());
                    }));
            }
        }

        #endregion
    }
}
