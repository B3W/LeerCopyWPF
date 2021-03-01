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

        /// <summary>
        /// Handle to dialog window controller
        /// </summary>
        private readonly IDialogWindowController _dialogWindowController;

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

        /// <summary>
        /// Last recorded location of the main window before it was hidden. Only valid when 'Hidden' is true
        /// </summary>
        private Point LastActiveLocation { get; set; }

        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructs instance of MainWindowController
        /// </summary>
        public MainWindowController(Window mainWindow, IDialogWindowController dialogWindowController)
        {
            _dialogWindowController = dialogWindowController;

            MainWindow = mainWindow;
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel(this);
            mainWindowViewModel.OpenSettingsEvent += (s, eargs) => new SettingsWindow().ShowDialog();
            MainWindow.DataContext = mainWindowViewModel;

            Hidden = true;
            LastActiveLocation = new Point(-1.0, -1.0);
        }


        public void Show()
        {
            Hidden = false;

            MainWindow.Show();
            MainWindow.WindowState = WindowState.Normal;
            MainWindow.Activate();
        }


        public void Hide()
        {
            Hidden = true;

            // Record location of the window before hiding it
            LastActiveLocation = new Point(MainWindow.Left, MainWindow.Top);

            MainWindow.WindowState = WindowState.Minimized;
            MainWindow.Hide();
        }


        public void Close()
        {
            MainWindow.Close();
        }


        public void PerformAction(MainWindowControllerActions action)
        {
            switch (action)
            {
                case MainWindowControllerActions.StartSelection:
                    Hide();                     // Hide the main window
                    StartSelection();           // Start the new selection
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
            SelectionStartEventArgs args = new SelectionStartEventArgs(LastActiveLocation);
            SelectionStarted?.Invoke(this, args);
        }

        #endregion

        #endregion // Methods
    }
}
