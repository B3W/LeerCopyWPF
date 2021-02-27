using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace LeerCopyWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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

            // Initialize window controller
            // TODO

            // Show the main window
            // First Window object instantiated in AppDomain, therefore sets MainWindow property of Application
            Views.MainWindow window = new Views.MainWindow();
            window.Show();
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
