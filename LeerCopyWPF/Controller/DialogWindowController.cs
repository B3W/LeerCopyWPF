using LeerCopyWPF.Enums;
using LeerCopyWPF.Utilities;
using LeerCopyWPF.ViewModels;
using LeerCopyWPF.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LeerCopyWPF.Controller
{
    public class DialogWindowController : IDialogWindowController
    {
        #region Fields

        #region Public Fields
        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields
        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties
        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructs instance of MainWindowController
        /// </summary>
        public DialogWindowController()
        {
        }


        /// <summary>
        /// Shows dialog window
        /// </summary>
        /// <param name="args">Arguments associated with event</param>
        public bool? ShowDialog(BaseViewModel dialogViewModel)
        {
            bool? result = null;

            if (dialogViewModel != null)
            {
                DialogWindow dialog = new DialogWindow
                {
                    DataContext = dialogViewModel
                };

                result = dialog.ShowDialog();
            }

            return result;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
