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

        private readonly ISelectionWindowController _selectionWindowController;

        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties
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
        public MainWindowController(ISelectionWindowController selectionWindowController)
        {
            _selectionWindowController = selectionWindowController;

            // First Window object instantiated in AppDomain sets MainWindow property of Application (set anyway to be safe)
            MainWindow = new MainWindow(this);
            Application.Current.MainWindow = MainWindow;

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            MainWindow.DataContext = mainWindowViewModel;

            _selectionWindowController.SelectionQuit += OnSelectionQuit;
        }


        public void PerformAction(MainWindowControllerActions action)
        {
            switch (action)
            {
                case MainWindowControllerActions.StartSelection:
                    StartSelection();
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
            // Hide the main window prior to screen capture
            HideMainWindow();

            if (!_selectionWindowController.StartSelection(MainWindow))
            {
                // Unable to start selection
                PerformAction(MainWindowControllerActions.ShowMainWindow);

                // TODO Log, show notification
            }

            // Makes sure main window shows up in taskbar/Alt+Tab menu
            MainWindow.Show();
        }


        private void QuitSelection()
        {
            // TODO
        }


        private void ShowMainWindow()
        {
            MainWindow.Show();
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
        }


        private void HideMainWindow()
        {
            MainWindow.WindowState = WindowState.Minimized;
            MainWindow.Hide();
        }


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
            PerformAction(MainWindowControllerActions.ShowMainWindow);
        }

        #endregion

        #endregion // Methods
    }
}
