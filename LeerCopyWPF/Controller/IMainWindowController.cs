using LeerCopyWPF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeerCopyWPF.Controller
{
    public interface IMainWindowController
    {
        /// <summary>
        /// Event that fires when a selection start signal is received.
        /// </summary>
        event EventHandler SelectionStarted;

        /// <summary>
        /// Shows the main window.
        /// </summary>
        void Show();

        /// <summary>
        /// Hides/Minimizes the main window.
        /// </summary>
        void Hide();

        /// <summary>
        /// Closes the main window.
        /// </summary>
        void Close();

        /// <summary>
        /// Signals window controller to perform specified action.
        /// </summary>
        /// <param name="action">Action to perform</param>
        void PerformAction(MainWindowControllerActions action);
    }
}
