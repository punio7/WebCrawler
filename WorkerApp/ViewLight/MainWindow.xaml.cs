using System.Windows;
using WebCrawler.WorkerApp.ViewLight.ViewModel;

namespace WebCrawler.WorkerApp.ViewLight
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource viewModelLocatorViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("viewModelLocatorViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // viewModelLocatorViewSource.Source = [generic data source]
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource viewModelLocatorViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("viewModelLocatorViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // viewModelLocatorViewSource.Source = [generic data source]
        }
    }
}