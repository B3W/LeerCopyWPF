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

using LeerCopyWPF.Models;
using LeerCopyWPF.Utilities;
using LeerCopyWPF.ViewModels;
using LeerCopyWPF.Views;
using Serilog;
using System;
using System.Collections.Generic;
using System.Windows;

namespace LeerCopyWPF.Controller
{
    public class SelectionWindowController : ISelectionWindowController
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
        /// Handle to the dialog window controller
        /// </summary>
        private readonly IDialogWindowController _dialogWindowController;

        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties

        public event EventHandler SelectionQuit;

        public IDialogWindowController DialogWindowController { get => _dialogWindowController; }

        /// <summary>
        /// Metadata on all screens in user's environment
        /// </summary>
        public IList<SimpleScreen> Screens { get; private set; }

        /// <summary>
        /// Flag indicating if a selection operation is currently active
        /// </summary>
        public bool SelectionActive { get; private set; }

        /// <summary>
        /// Flag indicating if selection operation is enabled
        /// </summary>
        public bool SelectionEnabled { get; private set; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties

        /// <summary>
        /// Collection of all selection windows
        /// </summary>
        private IList<Window> SelectionWindows { get; set; }

        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructs instance of SelectionWindowController
        /// </summary>
        public SelectionWindowController(IDialogWindowController dialogWindowController)
        {
            _logger = Log.ForContext<SelectionWindowController>();
            _dialogWindowController = dialogWindowController;

            SelectionActive = false;
            SelectionEnabled = false;
            SelectionWindows = new List<Window>();
        }


        public bool StartSelection(Window owner)
        {
            if (SelectionActive)
            {
                _logger.Debug("Attempted to start selection when one was already active");
                return true;
            }

            SelectionActive = true;
            SelectionEnabled = true;
            Screens = BitmapUtilities.DetectScreens();

            // Initialize selection window for each screen
            SelectionWindows.Clear();

            foreach (SimpleScreen screen in Screens)
            {
                // Construct Window and associated ViewModel for given screen
                Window selectionWindow = new SelectionWindow(this, screen.Bounds);
                SelectionViewModel selectionViewModel = new SelectionViewModel(selectionWindow, screen.Bounds);
                
                selectionWindow.DataContext = selectionViewModel;
                selectionWindow.Owner = owner;
                selectionWindow.ShowInTaskbar = false;

                SelectionWindows.Add(selectionWindow);

                // Show all selection windows at the same time
                selectionWindow.Show();

                _logger.Debug("Constructed selection window {Window}", selectionWindow);
            }

            return true;
        }


        public void GiveSelectionFocus(Window owner)
        {
            if (SelectionActive)
            {
                foreach (SelectionWindow selectionWindow in SelectionWindows)
                {
                    if (selectionWindow.ScreenBounds.Contains(owner.Left, owner.Top))
                    {
                        _logger.Debug("{Window} given focus", selectionWindow);

                        selectionWindow.Activate();
                        selectionWindow.Focus();

                        break;
                    }
                }
            }
        }


        public void EnableSelection()
        {
            if (SelectionActive)
            {
                _logger.Debug("Enabling selection");
                SelectionEnabled = true;

                // Enable all selection windows
                foreach (Window window in SelectionWindows)
                {
                    window.IsEnabled = true;
                    window.Focusable = true;
                }
            }
        }


        public void DisableSelection()
        {
            if (SelectionActive)
            {
                _logger.Debug("Disabling selection");
                SelectionEnabled = false;

                // Disable all selection windows
                foreach (Window window in SelectionWindows)
                {
                    window.Focusable = false;
                    window.IsEnabled = false;
                }
            }
        }


        public void StopSelection()
        {
            _logger.Debug("Quitting selection");
            SelectionActive = false;
            SelectionEnabled = false;

            // Dispose of all selection windows
            foreach (Window window in SelectionWindows)
            {
                window.Close();
            }

            SelectionWindows.Clear();
            SelectionQuit?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
