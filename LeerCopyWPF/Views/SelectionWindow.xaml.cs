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
