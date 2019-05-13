using LeerCopyWPF.Commands;
using LeerCopyWPF.Models;
using LeerCopyWPF.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace LeerCopyWPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        /// <summary>
        /// All screens in user's environment
        /// </summary>
        private IList<SimpleScreen> _screens;
        /// <summary>
        /// Command for opening settings window
        /// </summary>
        private ICommand _settingsCommand;
        #endregion // Fields

        #region Constants
        private const string ConstDisplayName = "Leer Copy";
        #endregion // Constants

        #region Properties
        public override string DisplayName { get => ConstDisplayName; }

        public IList<SimpleScreen> Screens { get => _screens ?? (_screens = BitmapUtilities.CaptureScreens()); }

        public int CurScreenIndex { get; private set; }

        public SimpleScreen CurrentScreen { get => Screens[CurScreenIndex]; }

        public event EventHandler OpenSettingsEvent;

        public ICommand SettingsCommand { get => _settingsCommand ?? (_settingsCommand = new RelayCommand(param => ShowSettings())); }

        public ICommand CloseCommand { get; }
        #endregion // Properties

        #region Constructors
        public MainWindowViewModel(Action<object> closeAction)
        {
            // Capture all screens
            _screens = BitmapUtilities.CaptureScreens();

            CurScreenIndex = 0;
            CloseCommand = new RelayCommand(closeAction);
        }
        #endregion // Constructors

        #region Functions
        /// <summary>
        /// Initializes screen index to screen that contains given coordinates
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>Index of screen cooridinates lie within, 0 otherwise</returns>
        public int InitScreenIndex(double x, double y)
        {
            int i;
            bool found = false;

            for (i = 0; i < Screens.Count; i++)
            {
                if (Screens[i].Bounds.Contains(x, y))
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                return (CurScreenIndex = i);
            }
            return (CurScreenIndex = 0);
        } // InitScreenIndex


        /// <summary>
        /// Increments the screen index
        /// </summary>
        /// <returns>New screen index</returns>
        public int IncrementScreen()
        {
            return (CurScreenIndex = (CurScreenIndex + 1) % Screens.Count);
        } // IncrementScreen


        /// <summary>
        /// Opens SettingsWindow
        /// </summary>
        public void ShowSettings()
        {
            OpenSettingsEvent?.Invoke(this, EventArgs.Empty);
        } // OpenSettings
        #endregion // Functions
    }
}
