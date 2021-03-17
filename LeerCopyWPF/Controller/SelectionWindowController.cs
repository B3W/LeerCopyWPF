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
        /// Flag indicating if a selection operation is currently active
        /// </summary>
        public bool SelectionActive { get; private set; }

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
        public SelectionWindowController()
        {
            SelectionActive = false;
            SelectionWindows = new List<Window>();
        }


        public bool StartSelection(double startScreenX, double startScreenY)
        {
            if (SelectionActive)
            {
                return true;
            }

            SelectionActive = true;
            Screens = BitmapUtilities.CaptureScreens();

            // Initialize selection window for each screen
            SelectionWindows.Clear();
            Window activeWindow = null;

            foreach (SimpleScreen screen in Screens)
            {
                // Construct Window and associated ViewModel for given screen
                Window selectionWindow = new SelectionWindow(this, screen.Bounds);
                SelectionViewModel selectionViewModel = new SelectionViewModel(selectionWindow, screen.Bounds);
                
                selectionWindow.DataContext = selectionViewModel;
                selectionWindow.Owner = Application.Current.MainWindow;

                SelectionWindows.Add(selectionWindow);

                // Show all selection windows at the same time
                selectionWindow.Show();

                if (screen.Bounds.Contains(startScreenX, startScreenY))
                {
                    activeWindow = selectionWindow;
                }
            }

            activeWindow?.Activate();

            return true;
        }


        public void QuitSelection()
        {
            SelectionActive = false;

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
