using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeerCopyWPF.Controller
{
    public class DialogEventArgs : EventArgs
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

        /// <summary>
        /// ViewModel to display in dialog View
        /// </summary>
        public ViewModels.BaseViewModel DialogViewModel { get; }

        /// <summary>
        /// Action to perform on dialog result
        /// </summary>
        public Action<bool?> HandleResult { get; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructs DialogEventArgs instance with no action to handle dialog result
        /// </summary>
        /// <param name="dialogViewModel">ViewModel to display in dialog View</param>
        public DialogEventArgs(ViewModels.BaseViewModel dialogViewModel) : this(dialogViewModel, null) { }


        /// <summary>
        /// Constructs DialogEventArgs instance with action for handling dialog result
        /// </summary>
        /// <param name="dialogViewModel">ViewModel to display in dialog View</param>
        /// <param name="handleResult">Pointer to function for handling dialog result</param>
        public DialogEventArgs(ViewModels.BaseViewModel dialogViewModel, Action<bool?> handleResult)
        {
            DialogViewModel = dialogViewModel;
            HandleResult = handleResult;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
