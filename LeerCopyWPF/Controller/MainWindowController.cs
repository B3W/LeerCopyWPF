using LeerCopyWPF.Enums;
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
        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties

        public event EventHandler SelectionStarted;

        /// <summary>
        /// Flag indicating if main window is hidden
        /// </summary>
        public bool Hidden { get; private set; }

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
        public MainWindowController()
        {
            // First Window object instantiated in AppDomain sets MainWindow property of Application (set anyway to be safe)
            MainWindow = new MainWindow(this);
            Application.Current.MainWindow = MainWindow;

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            MainWindow.DataContext = mainWindowViewModel;

            Hidden = true;
        }


        public void PerformAction(MainWindowControllerActions action)
        {
            switch (action)
            {
                case MainWindowControllerActions.StartSelection:
                    Hide();                     // Hide the main window
                    StartSelection();           // Start the new selection
                    break;

                case MainWindowControllerActions.ShowMainWindow:
                    Show();
                    break;

                case MainWindowControllerActions.HideMainWindow:
                    Hide();
                    break;

                case MainWindowControllerActions.CloseMainWindow:
                    Close();
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
            SelectionStarted?.Invoke(this, EventArgs.Empty);
        }


        private void Show()
        {
            Hidden = false;

            MainWindow.Show();
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
        }


        private void Hide()
        {
            Hidden = true;

            MainWindow.WindowState = WindowState.Minimized;
            MainWindow.Hide();
        }


        private void Close()
        {
            MainWindow.Close();
        }

        #endregion

        #endregion // Methods
    }
}
