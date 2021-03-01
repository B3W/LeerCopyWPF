using LeerCopyWPF.Commands;
using LeerCopyWPF.Controller;
using LeerCopyWPF.Enums;
using LeerCopyWPF.Models;
using LeerCopyWPF.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace LeerCopyWPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields

        /// <summary>
        /// Handle to controller for the main window
        /// </summary>
        private readonly IMainWindowController _mainWindowController;
        
        #endregion // Fields

        #region Constants
        
        private const string ConstDisplayName = "Leer Copy";

        #endregion // Constants

        #region Properties

        public override string DisplayName { get => ConstDisplayName; }

        public event EventHandler OpenSettingsEvent;

        /// <summary>
        /// Command for selection capture button press
        /// </summary>
        public ICommand SelectionCaptureCommand { get; }

        /// <summary>
        /// Command for opening the settings
        /// </summary>
        public ICommand SettingsCommand { get; }

        /// <summary>
        /// Command for closing the main window
        /// </summary>
        public ICommand CloseCommand { get; }

        #endregion // Properties


        #region Constructors
        
        public MainWindowViewModel(IMainWindowController mainWindowController)
        {
            _mainWindowController = mainWindowController;

            SelectionCaptureCommand = new RelayCommand(param => SelectionCaptureHandler());
            SettingsCommand = new RelayCommand(param => ShowSettingsHandler());
            CloseCommand = new RelayCommand(param => CloseHandler());
        }
        
        #endregion // Constructors

        #region Methods
        
        /// <summary>
        /// Signals window controller that selection capture was requested
        /// </summary>
        private void SelectionCaptureHandler()
        {
            _mainWindowController.PerformAction(MainWindowControllerActions.StartSelection);
        }

        /// <summary>
        /// Opens settings.
        /// </summary>
        private void ShowSettingsHandler()
        {
            OpenSettingsEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Signals window controller to close main window
        /// </summary>
        private void CloseHandler()
        {
            _mainWindowController.Close();
        }

        #endregion // Methods
    }
}
