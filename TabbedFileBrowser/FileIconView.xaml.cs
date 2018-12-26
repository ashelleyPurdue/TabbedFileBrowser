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
    /// Interaction logic for FileIconView.xaml
    /// </summary>
    public partial class FileIconView : UserControl
    {
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register
        (
            "FilePath", 
            typeof(string), 
            typeof(FileIconView), 
            new PropertyMetadata((s, a) =>
            {
                ((FileIconView)s).FilePath = (string)a.NewValue;
            })
        );

        public string FilePath
        {
            get => (string)GetValue(FilePathProperty);
            set
            {
                SetValue(FilePathProperty, value);
                LoadIcon(value);
            }
        }

        public FileIconView()
        {
            InitializeComponent();
        }

        private void LoadIcon(string path)
        {
            text.Text = "(Placeholder icon for " + path + ")";
        }
    }
}
