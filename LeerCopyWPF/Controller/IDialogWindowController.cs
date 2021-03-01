using LeerCopyWPF.ViewModels;
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
        /// Shows the dialog.
        /// </summary>
        /// <param name="viewModel">ViewModel associated with dialog</param>
        void ShowDialog(BaseViewModel viewModel);
    }
}
