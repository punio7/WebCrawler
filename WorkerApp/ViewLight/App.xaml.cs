using System.Windows;
using GalaSoft.MvvmLight.Threading;
using WebCrawler.WorkerApp.ViewLight.ViewModel;

namespace WebCrawler.WorkerApp.ViewLight
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            ViewModelLocator.Cleanup();
        }

        private void Application_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            ViewModelLocator.Cleanup();
        }
    }
}
