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
using Serilog;
using System.Reflection;

namespace LeerCopyWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Handle to logger for this source context
        /// </summary>
        private ILogger _logger;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize local AppData directory
            string rootAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string appDataPath = Path.Combine(rootAppDataPath, Assembly.GetExecutingAssembly().GetName().Name);

            LeerCopyWPF.Properties.Settings.Default.AppDataLoc = appDataPath;
            LeerCopyWPF.Properties.Settings.Default.Save();

            try
            {
                Directory.CreateDirectory(appDataPath);
            }
            catch (Exception)
            {
                MessageBox.Show("Fatal error prior to starting application's UI.\nExiting application...",
                                "Fatal Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);

                Environment.Exit(1);
            }

            // Setup logger
            const string ConstLogTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{SourceContext:l}] {Message:lj}{NewLine}{Exception}";
            string logPath = Path.Combine(appDataPath, "log-.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(logPath,
                              outputTemplate: ConstLogTemplate,         // Format for logs
                              rollingInterval: RollingInterval.Day,     // New log file created each day with date appended to file name
                              retainedFileCountLimit: 20,               // Most recent 20 log files will be retained
                              buffered: false)                          // Each log event is flushed to disk
                .CreateLogger();

            _logger = Log.ForContext<App>();

            // Record application version
            string appVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            _logger.Information("Application startup {Version}", appVersion);

            // Initialize window controllers
            IDialogWindowController dialogWindowController = new DialogWindowController();
            ISelectionWindowController selectionWindowController = new SelectionWindowController(dialogWindowController);
            IMainWindowController mainWindowController = new MainWindowController(selectionWindowController, dialogWindowController);

            // Show main window
            mainWindowController.ShowMainWindow();
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
                            _logger.Debug("Deleting expired temp 'edit' file {File}", file);
                            File.Delete(file);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Exception while deleting temp 'edit' files");
            }
            finally
            {
                Log.CloseAndFlush();
                base.OnExit(e);
            }
        }

        /// <summary>
        /// Handles any exceptions unhandled by the UI thread of the application.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            _logger.Fatal(e.Exception, "Unhandled exception on UI thread");
            Log.CloseAndFlush();

            string fatalMsg = "Fatal error encountered; application will exit."
                              + $" Check logs located in \"{LeerCopyWPF.Properties.Settings.Default.AppDataLoc}\" for details.";
            Models.Notification fatalNotification = new Models.Notification("Fatal Error", fatalMsg, NotificationType.Error);
            ViewModels.NotificationViewModel notificationViewModel = new ViewModels.NotificationViewModel(fatalNotification);

            DialogWindow dialog = new DialogWindow
            {
                DataContext = notificationViewModel
            };
            dialog.ShowDialog();

            // Prevent default unhandled exception processing of allowing WPF to handle it (continues application execution)
            // e.Handled = true;
        }
    }
}
