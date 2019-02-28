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
    public partial class Overlay : Window
    {
        private SelectControl selectControl;
        private BitmapSource bitmapSource;

        public Overlay(BitmapSource bitmap)
        {
            InitializeComponent();
            bitmapSource = bitmap;
            selectControl = new SelectControl(bitmap);

            SelectionImg.Visibility = Visibility.Hidden;
            SelectionImg.Source = bitmap;
            SelectionImg.Clip = new RectangleGeometry();

            this.PreviewMouseLeftButtonDown += Overlay_MouseLeftButtonDown;
            this.PreviewMouseLeftButtonUp += Overlay_MouseLeftButtonUp;
            this.PreviewMouseMove += Overlay_MouseMove;
        }

        private void Overlay_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectControl.StartSelection(e.GetPosition(this));
            UpdateDisplayedImage();
            SelectionImg.Visibility = Visibility.Visible;
        }

        private void Overlay_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            selectControl.StopSelection(e.GetPosition(this));
        }

        private void Overlay_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectControl.isSelecting)
            {
                selectControl.UpdateSelection(e.GetPosition(this));
                UpdateDisplayedImage();
            }
        }
        
        private void UpdateDisplayedImage()
        {
            SelectionImg.Clip = selectControl.GetSelectionGeometry();
        }
    }
}
