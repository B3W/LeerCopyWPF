using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeerCopyWPF.Controller
{
    public interface ISelectionWindowController
    {
        /// <summary>
        /// Event that fires when selection is quit.
        /// </summary>
        event EventHandler SelectionQuit;

        /// <summary>
        /// Starts selection operation if there is not one already active.
        /// </summary>
        /// <param name="activeScreenLocation">Selection should start on screen that contains this point</param>
        /// <returns>true if selection started successfully, false otherwise</returns>
        bool StartSelection(System.Windows.Point activeScreenLocation);

        /// <summary>
        /// Switches the selection screen if possible.
        /// </summary>
        void SwitchScreen();

        /// <summary>
        /// Exits selection operation.
        /// </summary>
        void QuitSelection();
    }
}
