using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using LeerCopyWPF.Controller;
using LeerCopyWPF.Views;
using LeerCopyWPF.Enums;

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

            // Initialize window controllers
            ISelectionWindowController selectionWindowController = new SelectionWindowController();
            IMainWindowController mainWindowController = new MainWindowController(selectionWindowController);

            // Show main window
            mainWindowController.PerformAction(MainWindowControllerActions.ShowMainWindow);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            const double NUM_DAYS_TO_EXPIRE = 1.0D;

            try
            {
                // Clean up temporary 'edit' files in AppData
                string appDataPath = LeerCopyWPF.Properties.Settings.Default.AppDataLoc;

                if (Directory.Exists(appDataPath))
                {
                    // Get files which have default file name/extension
                    string searchPattern = LeerCopyWPF.Properties.Settings.Default.DefaultFileName
                                         + "*" 
                                         + LeerCopyWPF.Properties.Settings.Default.DefaultSaveExt;

                    string[] files = Directory.GetFiles(appDataPath, searchPattern);

                    // Delete 'default' files that are 1+ days old
                    DateTime fileDT;
                    DateTime currentDT = DateTime.Now;

                    foreach (string file in files)
                    {
                        fileDT = File.GetCreationTime(file);

                        if (fileDT.AddDays(NUM_DAYS_TO_EXPIRE) < currentDT)
                        {
                            File.Delete(file);
                        }
                    }
                }
            }
            finally
            {
                base.OnExit(e);
            }
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
