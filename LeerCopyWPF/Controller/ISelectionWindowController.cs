using LeerCopyWPF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LeerCopyWPF.Controller
{
    public interface ISelectionWindowController
    {
        /// <summary>
        /// Event that fires when selection is quit
        /// </summary>
        event EventHandler SelectionQuit;

        /// <summary>
        /// Handle to dialog window controller
        /// </summary>
        IDialogWindowController DialogWindowController { get; }

        /// <summary>
        /// Starts selection operation if there is not one already active
        /// </summary>
        /// <param name="owner">Main window that owns selection screens</param>
        /// <returns>true if selection started successfully, false otherwise</returns>
        bool StartSelection(Window owner);

        /// <summary>
        /// Gives focus to selection window that is on same screen as designated owner if a selection
        /// is started, otherwise does nothing
        /// </summary>
        /// <param name="owner">Main window that owns selection screens</param>
        void GiveSelectionFocus(Window owner);

        /// <summary>
        /// Enables all selection windows
        /// </summary>
        void EnableSelection();

        /// <summary>
        /// Disables all selection windows
        /// </summary>
        void DisableSelection();

        /// <summary>
        /// Exits selection operation
        /// </summary>
        void StopSelection();
    }
}
