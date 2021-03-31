using LeerCopyWPF.Enums;
using LeerCopyWPF.Utilities;
using LeerCopyWPF.ViewModels;
using LeerCopyWPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _selectionWindowController = selectionWindowController;
            _dialogWindowController = dialogWindowController;

            // First Window object instantiated in AppDomain sets MainWindow property of Application (set anyway to be safe)
            MainWindow = new MainWindow(this);
            Application.Current.MainWindow = MainWindow;

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            MainWindow.DataContext = mainWindowViewModel;

            _selectionWindowController.SelectionQuit += OnSelectionQuit;
        }


        public void PerformAction(MainWindowControllerActions action, object data = null)
        {
            switch (action)
            {
                case MainWindowControllerActions.StartSelection:
                    StartSelection();
                    break;

                case MainWindowControllerActions.GiveSelectionFocus:
                    _selectionWindowController.GiveSelectionFocus(MainWindow);
                    break;

                case MainWindowControllerActions.ShowMainWindow:
                    ShowMainWindow();
                    break;

                case MainWindowControllerActions.HideMainWindow:
                    HideMainWindow();
                    break;

                case MainWindowControllerActions.CloseMainWindow:
                    CloseMainWindow();
                    break;

                default:
                    break;
            }
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods

        /// <summary>
        /// Logic to perform start selection action.
        /// </summary>
        private void StartSelection()
        {
            if (!_selectionWindowController.StartSelection(MainWindow))
            {
                // Unable to start selection
                ShowMainWindow();

                // TODO Log, show notification
            }

            // Make sure the main window shows up on task bar and in Alt+Tab menu
            MainWindow.Show();
        }


        /// <summary>
        /// Shows and activates the main window
        /// </summary>
        private void ShowMainWindow()
        {
            MainWindow.Show();
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
        }


        /// <summary>
        /// Minimizes the window and hides the taskbar icon
        /// </summary>
        private void HideMainWindow()
        {
            MainWindow.Hide();
        }


        /// <summary>
        /// Completely closes the main window
        /// </summary>
        private void CloseMainWindow()
        {
            MainWindow.Close();
        }


        /// <summary>
        /// Handler for SelectionQuit event
        /// </summary>
        /// <param name="sender">Object which invoked event</param>
        /// <param name="e">Arguments associated with event</param>
        private void OnSelectionQuit(object sender, EventArgs e)
        {
            ShowMainWindow();
        }

        #endregion

        #endregion // Methods
    }
}
