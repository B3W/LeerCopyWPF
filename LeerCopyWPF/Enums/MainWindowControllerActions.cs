using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeerCopyWPF.Enums
{
    /// <summary>
    /// All possible actions that can be performed by the main window controller
    /// </summary>
    public enum MainWindowControllerActions
    {
        ShowMainWindow,
        HideMainWindow,
        CloseMainWindow,
        StartSelection,
        GiveSelectionFocus
    }
}
