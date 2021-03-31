using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeerCopyWPF.Controller
{
    public interface IDialogWindowController
    {
        /// <summary>
        /// Shows dialog based on given arguments
        /// </summary>
        /// <param name="dialogViewModel">ViewModel to display in dialog</param>
        bool? ShowDialog(ViewModels.BaseViewModel dialogViewModel);
    }
}
