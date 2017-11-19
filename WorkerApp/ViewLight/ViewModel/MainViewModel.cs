using System;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Win32;
using ViewLight.Model;
using WebCrawler.Common.Enums;
using WebCrawler.WorkerApp.Common.Messages;
using WebCrawler.WorkerApp.Common.Model;
using WebCrawler.WorkerApp.Logic.Managers;
using WebCrawler.WorkerApp.ViewModel;

namespace WebCrawler.WorkerApp.ViewLight.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public ProcessListViewModel ProcessListViewModel { get; set; }
        public MainToolbarViewModel MainToolbarViewModel { get; set; }
        public ProcessOutputViewModel ProcessOutputViewModel { get; set; }
        public HubConnectionViewModel HubConnectionViewModel { get; set; }
        public ProcessManager ProcessManager { get; set; }
        private OpenFileDialog openExecutableDialog;


        public MainViewModel(IDataService dataService, ProcessListViewModel processListViewModel, MainToolbarViewModel mainToolbarViewModel, ProcessOutputViewModel processOutputViewModel, 
            HubConnectionViewModel hubConnectionViewModel, ProcessManager processManager)
        {
            ProcessListViewModel = processListViewModel;
            MainToolbarViewModel = mainToolbarViewModel;
            ProcessOutputViewModel = processOutputViewModel;
            HubConnectionViewModel = hubConnectionViewModel;
            ProcessManager = processManager;
            RegisterMessages();
            RegisterEvents();
            openExecutableDialog = new OpenFileDialog()
            {
                InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location,
                Filter = "Pliki wykonywalne (*.exe)|*.exe",
                FilterIndex = 1,
                RestoreDirectory = true,
            };
        }
        private void RegisterMessages()
        {
            MessengerInstance.Register<AddProcessMessage>(this, AddProcess);
            MessengerInstance.Register<StopProcessMessage>(this, StopProcess);
            MessengerInstance.Register<ProcessSelectionChangeMessage>(this, ProcessSelectionChange);
            MessengerInstance.Register<ExecuteCommandMessage>(this, ExecuteProcessCommand);
        }

        private void RegisterEvents()
        {
            ProcessManager.ProcessAdded += ProcessManager_ProcessAdded;
        }

        #region Events

        private void ProcessManager_ProcessAdded(ProcessInstance newProcess)
        {
            Application.Current.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                ProcessListViewModel.AddProcess(new ProcessViewModel()
                {
                    ProcessInstance = newProcess,
                });
            }));
        } 

        #endregion

        #region Messages
        private void AddProcess(AddProcessMessage message)
        {
            if (openExecutableDialog.ShowDialog() == true)
            {
                ProcessManager.CreateNewProcessForLocalhost(openExecutableDialog.FileName);
            }
        }

        private void StopProcess(StopProcessMessage message)
        {
            var process = ProcessListViewModel.SelectedProcess;
            if (process != null)
            {
                ProcessManager.StopProcess(process.SessionId);
                ProcessListViewModel.RemoveProcess(process);
            }
        }

        private void ProcessSelectionChange(ProcessSelectionChangeMessage message)
        {
            var process = ProcessListViewModel.SelectedProcess;
            if (process != null)
            {
                ProcessOutputViewModel.Enabled = true;
                ProcessOutputViewModel.OutputText = process.ProcessInstance.OutputBuffer;
            }
            else
            {
                ProcessOutputViewModel.OutputText.Clear();
                ProcessOutputViewModel.Enabled = false;
            }
        }

        private void ExecuteProcessCommand(ExecuteCommandMessage message)
        {
            var process = ProcessListViewModel.SelectedProcess;
            if (process != null)
            {
                ProcessManager.ExecuteCommand(process.SessionId, message.CommandText);
            }
        } 
        #endregion

        #region WindowClose Command

        private RelayCommand _windowClose;
        public RelayCommand WindowClose
        {
            get
            {
                return _windowClose
                    ?? (_windowClose = new RelayCommand(
                    () =>
                    {
                        Cleanup();
                    }));
            }
        }

        #endregion

        public override void Cleanup()
        {
            ProcessListViewModel.Cleanup();
            HubConnectionViewModel.Cleanup();
            base.Cleanup();
        }
    }
}