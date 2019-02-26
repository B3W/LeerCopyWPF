using System.Windows;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.Views
{
    /// <summary>
    /// Interaction logic for BitmapWindow.xaml
    /// </summary>
    public partial class BitmapWindow : Window
    {
        public BitmapWindow(BitmapSource bitmap)
        {
            InitializeComponent();

            BitmapImg.Source = bitmap;
        }
    }
}
