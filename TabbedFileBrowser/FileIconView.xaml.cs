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
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace TabbedFileBrowser
{
    /// <summary>
    /// Interaction logic for FileIconView.xaml
    /// </summary>
    public partial class FileIconView : UserControl
    {
        public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register
        (
            "File", 
            typeof(FileSystemInfo), 
            typeof(FileIconView), 
            new PropertyMetadata((s, a) =>
            {
                ((FileIconView)s).File = (FileSystemInfo)a.NewValue;
            })
        );

        public FileSystemInfo File
        {
            get => (FileSystemInfo)GetValue(FilePathProperty);
            set
            {
                SetValue(FilePathProperty, value);
                image.Source = LoadIcon(value);
            }
        }

        public FileIconView()
        {
            InitializeComponent();
        }

        private ImageSource LoadIcon(FileSystemInfo file)
        {
            // TODO: check if the icon exists in the cache first

            // Get the icon
            var bmp = Icon.ExtractAssociatedIcon(file.FullName).ToBitmap();

            using (var memory = new MemoryStream())
            {
                bmp.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }
    }
}
