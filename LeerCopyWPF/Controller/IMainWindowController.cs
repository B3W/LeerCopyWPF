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
        /// Handle to dialog window controller
        /// </summary>
        IDialogWindowController DialogWindowController { get; }

        /// <summary>
        /// Shows and activates main window
        /// </summary>
        void ShowMainWindow();

        /// <summary>
        /// Minimizes main window and hides taskbar icon
        /// </summary>
        void HideMainWindow();

        /// <summary>
        /// Completely closes main window
        /// </summary>
        void CloseMainWindow();

        /// <summary>
        /// Logic to start selection
        /// </summary>
        void StartSelection();

        /// <summary>
        /// Gives input focus to active selection
        /// </summary>
        void GiveSelectionFocus();
    }
}
