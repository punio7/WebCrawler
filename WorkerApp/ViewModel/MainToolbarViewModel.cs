using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WebCrawler.WorkerApp.Common.Messages;

namespace WebCrawler.WorkerApp.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainToolbarViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainToolbarViewModel class.
        /// </summary>
        public MainToolbarViewModel()
        {
        }

        #region AddProcess Command

        private RelayCommand _addProcess;

        /// <summary>
        /// Gets the AddProcess.
        /// </summary>
        public RelayCommand AddProcess
        {
            get
            {
                return _addProcess
                    ?? (_addProcess = new RelayCommand(
                    () =>
                    {
                        MessengerInstance.Send(new AddProcessMessage());
                    }));
            }
        }

        #endregion

        #region StopProcess Command

        private RelayCommand _stoProcess;

        /// <summary>
        /// Gets the StopProcess.
        /// </summary>
        public RelayCommand StopProcess
        {
            get
            {
                return _stoProcess
                    ?? (_stoProcess = new RelayCommand(
                    () =>
                    {
                        MessengerInstance.Send(new StopProcessMessage());
                    }));
            }
        }

        #endregion
    }
}