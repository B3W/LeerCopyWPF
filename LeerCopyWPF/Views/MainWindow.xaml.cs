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

using LeerCopyWPF.Utilities;
using LeerCopyWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
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
        private readonly MainWindowViewModel _mainWindowViewModel;
        /// <summary>
        /// Selection windows for each screen
        /// </summary>
        private SelectionWindow[] selectionWindows;
        /// <summary>
        /// Flag to prevent loaded event handler logic firing multiple times
        /// </summary>
        private bool winLoaded = false;


        public MainWindow()
        {
            // Register window lifetime event handlers
            this.Loaded += MainWindow_Loaded;

            InitializeComponent();

            _mainWindowViewModel = new MainWindowViewModel(param => this.Close());
            _mainWindowViewModel.OpenSettingsEvent += (s, eargs) => new SettingsWindow().ShowDialog();
            DataContext = _mainWindowViewModel;

            // Initialize container for selection windows
            selectionWindows = new SelectionWindow[_mainWindowViewModel.Screens.Count];
        }


        /// <summary>
        /// Initializes a selection window covering the given bounds
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        private SelectionWindow InitSelectWindow(Rect bounds)
        {
            SelectionWindow sWin = new SelectionWindow(bounds, (_mainWindowViewModel.Screens.Count > 1));
            sWin.SignalMain += Selection_SignalMain;
            sWin.Owner = this;

            return sWin;
        } // InitSelectWindow


        /// <summary>
        /// Event for child SelectionWindow to signal MainWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Selection_SignalMain(object sender, FlagEventArgs e)
        {
            if (e.Flag)
            {
                // Increment screen index
                int curScreenIndex =_mainWindowViewModel.IncrementScreen();

                // Create new selection window or show existing
                if (selectionWindows[curScreenIndex] == null)
                {
                    selectionWindows[curScreenIndex] = InitSelectWindow(_mainWindowViewModel.CurrentScreen.Bounds);
                }
                selectionWindows[curScreenIndex].Show();
                selectionWindows[curScreenIndex].Activate();
            }
            else
            {
                // Clear all selections when selection capture is quit
                for (int i = 0; i < selectionWindows.Length; i++)
                {
                    selectionWindows[i]?.Close();
                    selectionWindows[i] = null;
                }

                // Show MainWindow
                this.Show();
                this.WindowState = WindowState.Normal;
            }
        } // Selection_SignalMain


        private void SelectCaptureBtn_Click(object sender, RoutedEventArgs e)
        {
            // Determine screen MainWindow is located on
            // ***Needs to be done before window is minimized***
            int curScreenIndex = _mainWindowViewModel.InitScreenIndex(this.Left, this.Top);

            // Hide MainWindow
            this.WindowState = WindowState.Minimized;
            this.Hide();

            // Open SelectionWindow
            selectionWindows[curScreenIndex] = InitSelectWindow(_mainWindowViewModel.CurrentScreen.Bounds);
            selectionWindows[curScreenIndex].Show();
            selectionWindows[curScreenIndex].Activate();
        } // SelectCaptureBtn_Click


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!winLoaded)
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
                        this.Left = tmpL;
                        this.Top = tmpT;
                        break;
                    }
                }

                winLoaded = true;
            }
        } // MainWindow_Loaded


        protected override void OnClosing(CancelEventArgs e)
        {
            // Clean up temporary 'edit' files in AppData
            string appDataPath = Properties.Settings.Default.AppDataLoc;

            if (Directory.Exists(appDataPath))
            {
                // Get files which have default file name/extension
                string searchPattern = Properties.Settings.Default.DefaultFileName + "*" + Properties.Settings.Default.DefaultSaveExt;
                string[] files = Directory.GetFiles(appDataPath, searchPattern);

                // Delete 'default' files that are 1+ days old
                DateTime fileDT;
                DateTime currentDT = DateTime.Now;

                foreach (string file in files)
                {
                    fileDT = File.GetCreationTime(file);
                    if (fileDT.AddDays(1.0) < currentDT)
                    {
                        File.Delete(file);
                    }
                }
            }

            // Save settings
            Properties.Settings.Default.MainWinX = this.Left;
            Properties.Settings.Default.MainWinY = this.Top;
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        } // MainWindow_Closing
    }
}
