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

using LeerCopyWPF.Constants;
using LeerCopyWPF.Controller;
using LeerCopyWPF.Enums;
using LeerCopyWPF.Utilities;
using LeerCopyWPF.ViewModels;
using LeerCopyWPF.Views;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
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

namespace LeerCopyWPF.Views
{
    /// <summary>
    /// Interaction logic for SelectionWindow.xaml
    /// </summary>
    public partial class SelectionWindow : Window
    {
        #region Fields

        /// <summary>
        /// Handle to selection window controller
        /// </summary>
        private readonly ISelectionWindowController _selectionWindowController;

        /// <summary>
        /// Converter instance for converter Key object to string
        /// </summary>
        private readonly KeyConverter _keyConverter;

        #endregion // Fields


        #region Properties

        /// <summary>
        /// Bounds of screen on which selection window is located
        /// </summary>
        public Rect ScreenBounds { get; }

        #endregion // Properties


        #region Constructors

        public SelectionWindow(ISelectionWindowController selectionWindowController, Rect screenBounds)
        {
            // Register window lifetime events
            Loaded += SelectionWindow_Loaded;

            InitializeComponent();

            _selectionWindowController = selectionWindowController;
            _keyConverter = new KeyConverter();

            // Place form on correct screen
            ScreenBounds = screenBounds;
            Left = ScreenBounds.Left;
            Top = ScreenBounds.Top;

            // Subscribe to other window events
            PreviewKeyUp += SelectionWindow_PreviewKeyUp;
        }

        #endregion // Constructors


        #region EventHandlers

        protected override void OnClosing(CancelEventArgs e)
        {
            // Save settings
            Properties.Settings.Default.Save();

            base.OnClosing(e);
        }


        private void SelectionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Maximized;
        }


        /// <summary>
        /// Handles all key events that are not able to be handled in XAML through KeyTriggers without breaking MVVM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectionWindow_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            string keyStr = _keyConverter.ConvertToString(e.Key);

            if (keyStr == Properties.Settings.Default.SaveKey)          // Save selection key binding
            {
                SelectionViewModel viewModel = DataContext as SelectionViewModel;

                if (viewModel.CanSave)
                {
                    // Important that selection window opening save dialog is always topmost in z-order otherwise
                    // dragging the dialog to a different screen and clicking on that window would cover the dialog
                    Topmost = true;

                    // Disable selection on all other selection windows
                    _selectionWindowController.DisableSelection();

                    // Get the full path for the initial save directory (relative paths will throw exception)
                    string initialSavePath = System.IO.Path.GetFullPath(Properties.Settings.Default.LastSavePath);

                    // Configure save dialog
                    Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        AddExtension = true,
                        DefaultExt = Properties.Settings.Default.DefaultSaveExt,
                        FileName = Properties.Settings.Default.DefaultFileName,
                        Filter = "BMP (.bmp)|*.bmp|GIF (.gif)|*.gif|JPEG (.jpg)|*.jpg;*.jpeg|PNG (.png)|*.png|TIFF (.tif)|*.tif;*.tiff|WMP (.wmp)|*.wmp",
                        InitialDirectory = initialSavePath,
                        OverwritePrompt = true,
                        Title = "Save Leer"
                    };

                    // Show dialog
                    bool? result = saveDialog.ShowDialog(this);

                    if (result == true)
                    {
                        viewModel.SaveCommand.Execute(saveDialog.FileName);
                    }

                    // Enable selection on all other selection windows
                    _selectionWindowController.EnableSelection();

                    // Clear the topmost designation for normal operation and give focus to this window
                    Topmost = false;
                    Activate();
                    Focus();
                }

                e.Handled = true;
            }
            else if (keyStr == Properties.Settings.Default.SettingsWin) // Open settings key binding
            {
                SelectionViewModel viewModel = DataContext as SelectionViewModel;

                if (viewModel.CanOpenSettings)
                {
                    SettingsViewModel settingsViewModel = new SettingsViewModel();
                    _selectionWindowController.DialogWindowController.ShowDialog(settingsViewModel);
                }

                e.Handled = true;
            }
            else if (keyStr == Properties.Settings.Default.QuitKey)     // Quit selection key binding
            {
                SelectionViewModel viewModel = DataContext as SelectionViewModel;

                if (viewModel.CanClose)
                {
                    _selectionWindowController.StopSelection();
                }

                e.Handled = true;
            }
        }

        #endregion // EventHandlers
    }
}
