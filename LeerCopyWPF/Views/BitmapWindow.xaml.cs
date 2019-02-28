using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.Views
{
    /// <summary>
    /// Interaction logic for BitmapWindow.xaml
    /// </summary>
    public partial class BitmapWindow : Window
    {
        /// <summary>
        /// Bitmap of the screen to be displayed
        /// </summary>
        private BitmapSource bitmap;
        /// <summary>
        /// Guards against multiple 'Loaded' events firing
        /// </summary>
        private static bool loaded = false;

        public BitmapWindow()
        {
            this.Initialized += BitmapWindow_Initialized;
            this.Loaded += BitmapWindow_Loaded;
            this.ContentRendered += BitmapWindow_ContentRendered;
            InitializeComponent();
        }

        private void BitmapWindow_Initialized(object sender, EventArgs e)
        {
            bitmap = Utilities.BitmapUtilities.CaptureScreen();
        }

        private void BitmapWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loaded)
            {
                BitmapImg.Source = bitmap;
                loaded = true;
            }
        }

        private void BitmapWindow_ContentRendered(object sender, EventArgs e)
        {
            Overlay overlay = new Overlay(bitmap);
            overlay.Owner = this;
            overlay.ShowDialog();

            this.Close();
        }
    }
}
