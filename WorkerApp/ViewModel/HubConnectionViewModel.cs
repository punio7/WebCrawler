using System;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using Microsoft.AspNetCore.SignalR.Client;
using WebCrawler.WorkerApp.Logic.Managers;

namespace WebCrawler.WorkerApp.ViewModel
{
    public class HubConnectionViewModel : ViewModelBase
    {
        private HubConnectionManager HubConnectionManager { get; set; }
        private HubConnection hubConnection = null;
        public HubConnectionViewModel(HubConnectionManager hubConnectionManager)
        {
            HubConnectionManager = hubConnectionManager;
        }

        private class StateChange
        {
            public StateChange(HubConnectionState oldState, HubConnectionState newState)
            {
                OldState = oldState;
                NewState = newState;
            }
            public HubConnectionState OldState { get; set; }
            public HubConnectionState NewState { get; set; }
        }

        private string _hubUrl = @"https://localhost:44399/";
        public string HubUrl
        {
            get
            {
                return _hubUrl;
            }

            set
            {
                if (_hubUrl == value)
                {
                    return;
                }

                _hubUrl = value;
                RaisePropertyChanged(nameof(HubUrl));
            }
        }

        private bool _hubUrlEnabled = true;
        public bool HubUrlEnabled
        {
            get
            {
                return _hubUrlEnabled;
            }

            set
            {
                if (_hubUrlEnabled == value)
                {
                    return;
                }

                _hubUrlEnabled = value;
                RaisePropertyChanged(nameof(HubUrlEnabled));
            }
        }

        private bool _connectEnabled = true;
        public bool ConnectEnabled
        {
            get
            {
                return _connectEnabled;
            }

            set
            {
                if (_connectEnabled == value)
                {
                    return;
                }

                _connectEnabled = value;
                RaisePropertyChanged(nameof(ConnectEnabled));
            }
        }

        private bool _disconnectEnabled = false;
        public bool DisconnectEnabled
        {
            get
            {
                return _disconnectEnabled;
            }

            set
            {
                if (_disconnectEnabled == value)
                {
                    return;
                }

                _disconnectEnabled = value;
                RaisePropertyChanged(nameof(DisconnectEnabled));
            }
        }

        public HubConnectionState ConnectionState
        {
            get
            {
                return hubConnection != null ?
                    hubConnection.State : HubConnectionState.Disconnected;
            }
        }

        private RelayCommand _connect;

        public RelayCommand Connect
        {
            get
            {
                return _connect
                    ?? (_connect = new RelayCommand(
                    () =>
                    {
                        WaitCallback calback = new WaitCallback(HubconnectionCallback);
                        ThreadPool.QueueUserWorkItem(calback);
                    }));
            }
        }

        private async void HubconnectionCallback(object state)
        {
            try
            {
                Task<HubConnection> task = HubConnectionManager.CreateHubConnection(HubUrl);
                hubConnection = await task;
                //hubConnection.StateChanged += HubConnection_StateChanged;
                //hubConnection.Error += HubConnection_Error;
                HubConnection_StateChanged(new StateChange(HubConnectionState.Disconnected, HubConnectionState.Connected));
                //RaisePropertyChanged(nameof(ConnectionState));
            }
            catch (Exception e)
            {
                HandleSignalRError(e);
            }
        }

        private void HubConnection_Error(Exception e)
        {
            HandleSignalRError(e);
        }

        private RelayCommand _disconnect;
        public RelayCommand Disconnect
        {
            get
            {
                return _disconnect
                    ?? (_disconnect = new RelayCommand(
                    async () =>
                    {
                        try
                        {
                            await HubConnectionManager.Disconnect();
                            HubConnection_StateChanged(new StateChange(HubConnectionState.Connected, HubConnectionState.Disconnected));
                            RaisePropertyChanged(nameof(ConnectionState));
                            _connect.RaiseCanExecuteChanged();
                        }
                        catch (Exception e)
                        {
                            HandleSignalRError(e);
                        }
                    }));
            }
        }

        private void HubConnection_StateChanged(StateChange args)
        {
            RaisePropertyChanged(nameof(ConnectionState));
            if (args.NewState == HubConnectionState.Disconnected)
            {
                ConnectEnabled = true;
                DisconnectEnabled = false;
            }
            if (args.NewState != HubConnectionState.Disconnected)
            {
                ConnectEnabled = false;
                DisconnectEnabled = true;
            }
        }

        public override void Cleanup()
        {
            HubConnectionManager.Dispose();
            base.Cleanup();
        }
    }
}