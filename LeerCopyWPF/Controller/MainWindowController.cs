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

using LeerCopyWPF.Enums;
using LeerCopyWPF.Models;
using LeerCopyWPF.ViewModels;
using LeerCopyWPF.Views;
using Serilog;
using System;
using System.Windows;

namespace LeerCopyWPF.Controller
{
    public class MainWindowController : IMainWindowController
    {
        #region Fields

        #region Public Fields
        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields

        /// <summary>
        /// Handle to logger for this source context
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Handle to the selection window controller
        /// </summary>
        private readonly ISelectionWindowController _selectionWindowController;

        /// <summary>
        /// Handle to the dialog window controller
        /// </summary>
        private readonly IDialogWindowController _dialogWindowController;

        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties

        public IDialogWindowController DialogWindowController { get => _dialogWindowController; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties

        /// <summary>
        /// Handle to the main window of the application
        /// </summary>
        private Window MainWindow { get; }

        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructs instance of MainWindowController
        /// </summary>
        public MainWindowController(ISelectionWindowController selectionWindowController, IDialogWindowController dialogWindowController)
        {
            _logger = Log.ForContext<MainWindowController>();
            _selectionWindowController = selectionWindowController;
            _dialogWindowController = dialogWindowController;

            // First Window object instantiated in AppDomain sets MainWindow property of Application (set anyway to be safe)
            MainWindow = new MainWindow(this);
            Application.Current.MainWindow = MainWindow;

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            MainWindow.DataContext = mainWindowViewModel;

            _selectionWindowController.SelectionQuit += OnSelectionQuit;
        }

        
        public void ShowMainWindow()
        {
            _logger.Debug("Showing main window");

            MainWindow.Show();
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
        }


        public void HideMainWindow()
        {
            _logger.Debug("Hiding main window");
            MainWindow.Hide();
        }


        /// <summary>
        /// Completely closes the main window
        /// </summary>
        public void CloseMainWindow()
        {
            _logger.Debug("Closing main window");
            MainWindow.Close();
        }


        public void StartSelection()
        {
            if (!_selectionWindowController.StartSelection(MainWindow))
            {
                // Unable to start selection
                _logger.Error("Unable to start selection");

                // Show notification then re-show main window
                Notification notification = new Notification("Error", "Unable to start selection. Please try again.", NotificationType.Error);
                NotificationViewModel notificationViewModel = new NotificationViewModel(notification);
                _dialogWindowController.ShowDialog(notificationViewModel);                

                ShowMainWindow();
            }

            // Make sure the main window shows up on task bar and in Alt+Tab menu
            MainWindow.Show();
        }


        public void GiveSelectionFocus()
        {
            _selectionWindowController.GiveSelectionFocus(MainWindow);
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods

        /// <summary>
        /// Handler for SelectionQuit event
        /// </summary>
        /// <param name="sender">Object which invoked event</param>
        /// <param name="e">Arguments associated with event</param>
        private void OnSelectionQuit(object sender, EventArgs e)
        {
            _logger.Debug("Selection quit");
            ShowMainWindow();
        }

        #endregion

        #endregion // Methods
    }
}
