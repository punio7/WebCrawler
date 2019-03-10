/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:ViewLight.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using ViewLight.Model;
using WebCrawler.WorkerApp.Logic.Factories;
using WebCrawler.WorkerApp.Logic.Managers;
using WebCrawler.WorkerApp.ViewModel;

namespace WebCrawler.WorkerApp.ViewLight.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.mvvmlight.net
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<ProcessInstanceFactory>();
            SimpleIoc.Default.Register<ApplicationsManager>();
            SimpleIoc.Default.Register<ProcessManager>();
            SimpleIoc.Default.Register<HubConnectionManager>();

            SimpleIoc.Default.Register<HubConnectionViewModel>();
            SimpleIoc.Default.Register<ProcessViewModel>();
            SimpleIoc.Default.Register<ProcessListViewModel>();
            SimpleIoc.Default.Register<MainToolbarViewModel>();
            SimpleIoc.Default.Register<ProcessOutputViewModel>();
            SimpleIoc.Default.Register<MainViewModel>();
        }

        /// <summary>
        /// Gets the Main property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the Process property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ProcessViewModel Process
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProcessViewModel>();
            }
        }

        /// <summary>
        /// Gets the ProcessList property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ProcessListViewModel ProcessList
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProcessListViewModel>();
            }
        }

        /// <summary>
        /// Gets the ViewModelPropertyName property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainToolbarViewModel MainToolbar
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainToolbarViewModel>();
            }
        }

        /// <summary>
        /// Gets the ProcessOutput property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ProcessOutputViewModel ProcessOutput
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProcessOutputViewModel>();
            }
        }

        /// <summary>
        /// Gets the HubConnection property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public HubConnectionViewModel HubConnection
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HubConnectionViewModel>();
            }
        }

        public static void Cleanup()
        {
            ServiceLocator.Current.GetInstance<MainViewModel>().Cleanup();
        }
    }
}