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
        /// <returns>true if selection started successfully, false otherwise</returns>
        bool StartSelection();

        /// <summary>
        /// Exits selection operation.
        /// </summary>
        void QuitSelection();
    }
}
