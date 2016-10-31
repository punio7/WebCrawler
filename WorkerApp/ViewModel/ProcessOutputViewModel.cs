using System;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using WebCrawler.WorkerApp.Common;
using WebCrawler.WorkerApp.Common.Messages;

namespace WebCrawler.WorkerApp.ViewModel
{
    public class ProcessOutputViewModel : ViewModelBase
    {
        public ProcessOutputViewModel()
        {
        }


        private bool _enabled = false;
        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                if (_enabled == value)
                {
                    return;
                }

                _enabled = value;
                RaisePropertyChanged(nameof(Enabled));
            }
        }


        private string _commandText = string.Empty;
        public string CommandText
        {
            get
            {
                return _commandText;
            }

            set
            {
                if (_commandText == value)
                {
                    return;
                }

                _commandText = value;
                RaisePropertyChanged(nameof(CommandText));
            }
        }

        private ObservableStringBuilder _outputText = new ObservableStringBuilder();
        public ObservableStringBuilder OutputText
        {
            get
            {
                return _outputText;
            }

            set
            {
                if (_outputText == value)
                {
                    return;
                }

                _outputText = value;
                RaisePropertyChanged(nameof(OutputText));
            }
        }

        #region OutputTexEnterKey Command

        private RelayCommand _commandTextEnterKey;
        public RelayCommand CommandTextEnterKey
        {
            get
            {
                return _commandTextEnterKey
                    ?? (_commandTextEnterKey = new RelayCommand(
                    () =>
                    {
                        MessengerInstance.Send(new ExecuteCommandMessage()
                        {
                            CommandText = CommandText,
                        });
                        CommandText = String.Empty;
                    }));
            }
        }

        #endregion
    }
}