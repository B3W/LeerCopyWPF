using LeerCopyWPF.Utilities;
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
        /// <param name="startScreenX">X coordinate located on active screen</param>
        /// <param name="startScreenY">Y coordinate located on active screen</param>
        /// <returns>true if selection started successfully, false otherwise</returns>
        bool StartSelection(double startScreenX, double startScreenY);

        /// <summary>
        /// Exits selection operation.
        /// </summary>
        void QuitSelection();
    }
}
