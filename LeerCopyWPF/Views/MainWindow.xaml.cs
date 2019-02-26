using LeerCopyWPF.Controllers;
using LeerCopyWPF.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeerCopyWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SelectControl selectControl;
        private BitmapWindow bitmapWindow;

        public MainWindow()
        {
            this.Initialized += MainWindow_Initialized;
            InitializeComponent();
            this.ContentRendered += MainWindow_ContentRendered;
        }

        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            BitmapSource bitmap = Utilities.BitmapUtilities.CaptureScreen();
            selectControl = new SelectControl(bitmap);

            bitmapWindow = new BitmapWindow(bitmap);
            bitmapWindow.Show();
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            bitmapWindow.Owner = this;
        }
    }
}
