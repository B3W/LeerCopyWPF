using LeerCopyWPF.Controllers;
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
        /// <summary>
        /// Controller for making selections
        /// </summary>
        private SelectControl selectControl;
        /// <summary>
        /// Bitmap of the screen
        /// </summary>
        private BitmapSource bitmapSource;
        /// <summary>
        /// Guard against load firing multiple times
        /// </summary>
        private static bool loaded = false;


        public MainWindow()
        {
            this.Initialized += MainWindow_Initialized;
            this.Loaded += MainWindow_Loaded;

            InitializeComponent();

            SelectionImg.Visibility = Visibility.Hidden;
            SelectionImg.Clip = new RectangleGeometry();

            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
            this.PreviewMouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            this.PreviewMouseLeftButtonUp += MainWindow_MouseLeftButtonUp;
            this.PreviewMouseMove += MainWindow_MouseMove;
        } // MainWindow


        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // TODO: BIND SETTINGS TO PROPERTIES FOR MANIPULATION

        } // MainWindow_PreviewKeyDown


        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectControl.StartSelection(e.GetPosition(this));
            UpdateDisplayedImage();
            SelectionImg.Visibility = Visibility.Visible;
        } // MainWindow_MouseLeftButtonDown


        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectControl.StopSelection(e.GetPosition(this));
        } // MainWindow_MouseLeftButtonUp


        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectControl.isSelecting)
            {
                selectControl.UpdateSelection(e.GetPosition(this));
                UpdateDisplayedImage();
            }
        } // MainWindow_MouseMove


        private void UpdateDisplayedImage()
        {
            SelectionImg.Clip = selectControl.GetSelectionGeometry();
        } // UpdateDisplayedImage


        private void MainWindow_Initialized(object sender, EventArgs e)
        {
            bitmapSource = Utilities.BitmapUtilities.CaptureScreen();
        } // MainWindow_Initialized


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!loaded)
            {
                ScreenImg.Source = bitmapSource;
                SelectionImg.Source = bitmapSource;
                selectControl = new SelectControl(bitmapSource);

                loaded = true;
            }
        } // MainWindow_Loaded
    }
}
