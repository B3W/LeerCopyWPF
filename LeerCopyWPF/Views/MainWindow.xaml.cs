/*
 * Leer Copy - Quick and Accurate Screen Capturing Application
 * Copyright (C) 2021  Weston Berg
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using LeerCopyWPF.Controller;
using LeerCopyWPF.Enums;
using LeerCopyWPF.Utilities;
using LeerCopyWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LeerCopyWPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Fields

        #region Public Fields
        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields

        /// <summary>
        /// Amount of time to wait before starting manual selection
        /// </summary>
        private const int _SELECT_CAPTURE_START_DELAY_MS = 300;

        /// <summary>
        /// Handle to the main window controller
        /// </summary>
        private readonly IMainWindowController _mainWindowController;

        /// <summary>
        /// Flag to prevent loaded event handler logic firing multiple times
        /// </summary>
        private bool _winLoaded = false;

        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties
        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        public MainWindow(IMainWindowController mainWindowController)
        {
            // Register window lifetime event handlers
            Loaded += MainWindow_Loaded;
            Activated += MainWindow_Activated;

            InitializeComponent();

            _mainWindowController = mainWindowController;
        }

        #endregion

        #region Protected Methods

        protected override void OnClosing(CancelEventArgs e)
        {
            // Save settings
            Properties.Settings.Default.MainWinX = Left;
            Properties.Settings.Default.MainWinY = Top;
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        }

        #endregion

        #region Private Methods

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!_winLoaded)
            {
                // Restore window location if possible
                double tmpL = Properties.Settings.Default.MainWinX;
                double tmpT = Properties.Settings.Default.MainWinY;
                System.Drawing.Point pt = new System.Drawing.Point((int)tmpL, (int)tmpT);

                // Check if a screen can contain window location
                foreach (System.Windows.Forms.Screen screen in System.Windows.Forms.Screen.AllScreens)
                {
                    if (screen.Bounds.Contains(pt))
                    {
                        Left = tmpL;
                        Top = tmpT;
                        break;
                    }
                }

                _winLoaded = true;
            }
        }


        private void MainWindow_Activated(object sender, EventArgs e)
        {
            // Give focus to an active selection, otherwise do nothing
            _mainWindowController.GiveSelectionFocus();
        }


        private async void SelectCaptureBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowController.HideMainWindow();
            await Task.Delay(_SELECT_CAPTURE_START_DELAY_MS); // Wait for window to hide before starting selection

            _mainWindowController.StartSelection();
        }


        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsViewModel settingsViewModel = new SettingsViewModel();
            _mainWindowController.DialogWindowController.ShowDialog(settingsViewModel);
        }


        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowController.CloseMainWindow();
        }

        #endregion

        #endregion // Methods
    }
}
