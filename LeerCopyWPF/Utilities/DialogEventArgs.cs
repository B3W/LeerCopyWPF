/*
 * Leer Copy - Quick and Accurate Screen Capturing Application
 * Copyright (C) 2021  Weston Berg
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeerCopyWPF.Utilities
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
