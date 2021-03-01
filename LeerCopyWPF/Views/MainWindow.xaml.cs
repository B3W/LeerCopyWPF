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
        /// <summary>
        /// Flag to prevent loaded event handler logic firing multiple times
        /// </summary>
        private bool winLoaded = false;


        public MainWindow()
        {
            // Register window lifetime event handlers
            this.Loaded += MainWindow_Loaded;

            InitializeComponent();
        }


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
