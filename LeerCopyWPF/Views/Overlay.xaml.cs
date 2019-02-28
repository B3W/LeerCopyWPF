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
        }

    }
}
