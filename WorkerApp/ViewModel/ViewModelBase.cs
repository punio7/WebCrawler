using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WebCrawler.WorkerApp.ViewModel
{
    public class ViewModelBase : GalaSoft.MvvmLight.ViewModelBase
    {
        protected void HandleError(Exception exception)
        {
            MessageBox.Show(exception.ToString(), "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        protected void HandleSignalRError(Exception exception)
        {
            if (exception.InnerException != null)
            {
                HandleError(exception.InnerException);
            }
            else
            {
                HandleError(exception);
            }
        }
    }
}
