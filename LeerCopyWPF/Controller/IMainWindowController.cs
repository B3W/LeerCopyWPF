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
        /// Signals window controller to perform specified action.
        /// </summary>
        /// <param name="action">Action to perform</param>
        void PerformAction(MainWindowControllerActions action, object data = null);
    }
}
