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

using LeerCopyWPF.Models;
using LeerCopyWPF.Utilities;
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
        /// <summary>
        /// All screens in user's environment
        /// </summary>
        private List<SimpleScreen> screens;
        /// <summary>
        /// Currently displayed screen for capture
        /// </summary>
        private int curScreenIndex = 0;
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

            // Initialize AppData setting
            try
            {
                Properties.Settings.Default.AppDataLoc = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                        System.IO.Path.DirectorySeparatorChar + Application.ResourceAssembly.GetName().Name + System.IO.Path.DirectorySeparatorChar;
                Properties.Settings.Default.Save();
            }
            catch (PlatformNotSupportedException)
            {
                // TODO exception handling
            }
            catch (InvalidOperationException)
            {
                // TODO exception handling
            }

            // Capture all screens
            screens = BitmapUtilities.CaptureScreens();

            // Initialize container for selection windows
            selectionWindows = new SelectionWindow[screens.Count];
        }


        /// <summary>
        /// Determines which screen the MainWindow is located on
        /// </summary>
        /// <returns>Index within 'screens' containing MainWindow if found, 0 otherwise</returns>
        private int GetMainScreenIndex()
        {
            int i;
            bool found = false;

            for (i = 0; i < screens.Count; i++)
            {
                if (screens[i].Bounds.Contains(new Point(this.Left, this.Top)))
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                return i;
            }
            return 0;
        } // GetMainScreenIndex


        /// <summary>
        /// Initializes a selection window covering the given bounds
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        private SelectionWindow InitSelectWindow(Rect bounds)
        {
            SelectionWindow sWin = new SelectionWindow(bounds, (screens.Count > 1));
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
                curScreenIndex = (curScreenIndex + 1) % screens.Count;
                // Create new selection window or show existing
                if (selectionWindows[curScreenIndex] == null)
                {
                    selectionWindows[curScreenIndex] = InitSelectWindow(screens[curScreenIndex].Bounds);
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
            curScreenIndex = GetMainScreenIndex();

            // Hide MainWindow
            this.WindowState = WindowState.Minimized;
            this.Hide();

            // Open SelectionWindow
            selectionWindows[curScreenIndex] = InitSelectWindow(screens[curScreenIndex].Bounds);
            selectionWindows[curScreenIndex].Show();
            selectionWindows[curScreenIndex].Activate();
        } // SelectCaptureBtn_Click


        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        } // ExitMenuItem_Click


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
