/*
 *  Leer Copy - Quick and Accurate Screen Capturing Application
 *  Copyright (C) 2019  Weston Berg
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
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
            _mainWindowController.PerformAction(MainWindowControllerActions.GiveSelectionFocus);
        }


        private async void SelectCaptureBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowController.PerformAction(MainWindowControllerActions.HideMainWindow);
            await Task.Delay(_SELECT_CAPTURE_START_DELAY_MS); // Wait for window to hide before starting selection

            _mainWindowController.PerformAction(MainWindowControllerActions.StartSelection);
        }


        private void OpenSettings_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().ShowDialog();
        }


        private void ExitApp_Click(object sender, RoutedEventArgs e)
        {
            _mainWindowController.PerformAction(MainWindowControllerActions.CloseMainWindow);
        }

        #endregion

        #endregion // Methods
    }
}
