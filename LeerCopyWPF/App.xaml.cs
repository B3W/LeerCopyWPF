using System;
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
        void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // Process unhandled exception
            Console.WriteLine("Unhandled Exception: %s\n", e.Exception.Message);
            // Prevent default unhandled exception processing
            e.Handled = true;
        }
    }
}
