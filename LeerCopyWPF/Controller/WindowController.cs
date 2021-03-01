﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeerCopyWPF.Controller
{
    /// <summary>
    /// Top-level window controller
    /// </summary>
    public class WindowController
    {
        #region Fields

        #region Public Fields
        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields
        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties

        /// <summary>
        /// Controller for the main window
        /// </summary>
        public IMainWindowController MainWindowController { get; }

        /// <summary>
        /// Controller for selection windows
        /// </summary>
        public ISelectionWindowController SelectionController { get; }

        /// <summary>
        /// Controller for dialog windows
        /// </summary>
        public IDialogWindowController DialogController { get; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructs WindowController instance
        /// </summary>
        public WindowController(IMainWindowController mainWindowController,
                                ISelectionWindowController selectionController,
                                IDialogWindowController dialogController)
        {
            MainWindowController = mainWindowController;
            SelectionController = selectionController;
            DialogController = dialogController;

            // Register event handlers
            MainWindowController.SelectionStarted += OnSelectionStart;
            SelectionController.SelectionQuit += OnSelectionQuit;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods

        /// <summary>
        /// Handler for SelectionStarted event
        /// </summary>
        /// <param name="sender">Object which invoked event</param>
        /// <param name="e">Arguments associated with event</param>
        private void OnSelectionStart(object sender, EventArgs e)
        {
            SelectionStartEventArgs startArgs = (SelectionStartEventArgs)e;

            if (!SelectionController.StartSelection(startArgs.ActiveScreenLocation))
            {
                // Unable to start selection
                MainWindowController.Show();

                // TODO Log, show notification
            }
        }


        /// <summary>
        /// Handler for SelectionQuit event
        /// </summary>
        /// <param name="sender">Object which invoked event</param>
        /// <param name="e">Arguments associated with event</param>
        private void OnSelectionQuit(object sender, EventArgs e)
        {
            MainWindowController.Show();
        }

        #endregion

        #endregion // Methods
    }
}