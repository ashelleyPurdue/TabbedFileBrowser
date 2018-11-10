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

namespace TabbedFileBrowser
{
    /// <summary>
    /// Interaction logic for TabbedFileBrowser.xaml
    /// </summary>
    public partial class TabbedFileBrowserControl : UserControl
    {
        private ITabbedFileBrowserViewModel ViewModel { get; set; }

        public TabbedFileBrowserControl()
        {
            InitializeComponent();

            ViewModel = new TabbedFileBrowserViewModel();
            DataContext = ViewModel;
        }
    }
}
