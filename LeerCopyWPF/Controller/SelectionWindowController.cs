using LeerCopyWPF.Models;
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
    public class SelectionWindowController : ISelectionWindowController
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

        public event EventHandler SelectionQuit;

        /// <summary>
        /// Metadata on all screens in user's environment
        /// </summary>
        public IList<SimpleScreen> Screens { get; private set; }

        /// <summary>
        /// The currently active screen.
        /// </summary>
        public SimpleScreen ActiveScreen { get => Screens[CurScreenIndex]; }

        /// <summary>
        /// Flag indicating if a selection operation is currently active
        /// </summary>
        public bool SelectionActive { get; private set; }

        /// <summary>
        /// Flag indicating if screen switching is enabled
        /// </summary>
        public bool ScreenSwitchEnabled { get; private set; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties

        /// <summary>
        /// Index of the currently active screen in the 'Screens' list
        /// </summary>
        private int CurScreenIndex { get; set; }

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
            _dialogWindowController = dialogWindowController;

            SelectionActive = false;
            ScreenSwitchEnabled = false;
            CurScreenIndex = -1;
            SelectionWindows = new List<Window>();
        }


        public bool StartSelection(Point activeScreenLocation)
        {
            if (SelectionActive)
            {
                return true;
            }

            SelectionActive = true;
            Screens = BitmapUtilities.CaptureScreens();
            CurScreenIndex = -1;
            ScreenSwitchEnabled = Screens.Count > 1;

            // Initialize selection window for each screen
            SelectionWindows.Clear();
            int screenIndex = 0;

            foreach (SimpleScreen screen in Screens)
            {
                Window selectionWindow = new SelectionWindow(screen.Bounds, ScreenSwitchEnabled);
                SelectionViewModel selectionViewModel = new SelectionViewModel(selectionWindow, screen.Bounds, this);
                selectionViewModel.OpenSettingsEvent += (s, eargs) => new SettingsWindow().ShowDialog();
                
                selectionWindow.DataContext = selectionViewModel;
                selectionWindow.Owner = Application.Current.MainWindow;

                SelectionWindows.Add(selectionWindow);

                // Check if this is the active screen
                if (screen.Bounds.Contains(activeScreenLocation))
                {
                    CurScreenIndex = screenIndex;
                }

                screenIndex++;
            }

            // Attempt to activate selection window
            bool success = CurScreenIndex >= 0;

            if (success)
            {
                SelectionWindows[CurScreenIndex].Show();
                SelectionWindows[CurScreenIndex].Activate();
            }

            return success;
        }


        public void SwitchScreen()
        {
            if (!ScreenSwitchEnabled)
            {
                return;
            }

            // Hide current screen
            SelectionWindows[CurScreenIndex].Hide();

            // Increment to the next screen index and show it
            CurScreenIndex = (CurScreenIndex + 1) % Screens.Count;

            SelectionWindows[CurScreenIndex].Show();
            SelectionWindows[CurScreenIndex].Activate();
        }


        public void QuitSelection()
        {
            SelectionActive = false;
            ScreenSwitchEnabled = false;

            // Dispose of all selection windows
            foreach (Window window in SelectionWindows)
            {
                window.Close();
            }

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
