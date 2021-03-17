using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using LeerCopyWPF.Controller;
using LeerCopyWPF.Views;

namespace LeerCopyWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Keep handle to window controller so it stays around for duration of application.
        /// </summary>
        private WindowController _windowController;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize AppData setting
            try
            {
                string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                LeerCopyWPF.Properties.Settings.Default.AppDataLoc = Path.Combine(appDataPath, ResourceAssembly.GetName().Name);

                LeerCopyWPF.Properties.Settings.Default.Save();
            }
            catch (PlatformNotSupportedException ex)
            {
                // TODO exception handling
                Console.WriteLine("App.OnStartup: Exception initializing AppData - %s\n", ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // TODO exception handling
                Console.WriteLine("App.OnStartup: Exception initializing AppData - %s\n", ex.Message);
            }

            // Construct the main window
            // First Window object instantiated in AppDomain sets MainWindow property of Application
            Window mainWindow = new MainWindow();

            // Initialize window controller
            IMainWindowController mainWindowController = new MainWindowController(mainWindow);
            ISelectionWindowController selectionWindowController = new SelectionWindowController();

            _windowController = new WindowController(mainWindowController, selectionWindowController);

            // Show main window
            _windowController.MainWindowController.Show();
        }

        /// <summary>
        /// Handles any exceptions unhandled by the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            Console.WriteLine("Unhandled Exception: %s\n", e.Exception.Message);

            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
