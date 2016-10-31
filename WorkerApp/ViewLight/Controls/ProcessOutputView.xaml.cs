using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WebCrawler.WorkerApp.ViewLight.Controls
{
    /// <summary>
    /// Interaction logic for ProcessOutputView.xaml
    /// </summary>
    public partial class ProcessOutputView : UserControl
    {
        public ProcessOutputView()
        {
            InitializeComponent();
        }

        private void processOutputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            processOutputTextBox.CaretIndex = processOutputTextBox.Text.Length;
            processOutputTextBox.ScrollToEnd();
        }
    }
}
