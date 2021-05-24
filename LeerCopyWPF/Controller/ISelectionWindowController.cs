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
using System.Windows;

namespace LeerCopyWPF.Controller
{
    public interface ISelectionWindowController
    {
        /// <summary>
        /// Event that fires when selection is quit
        /// </summary>
        event EventHandler SelectionQuit;

        /// <summary>
        /// Handle to dialog window controller
        /// </summary>
        IDialogWindowController DialogWindowController { get; }

        /// <summary>
        /// Starts selection operation if there is not one already active
        /// </summary>
        /// <param name="owner">Main window that owns selection screens</param>
        /// <returns>true if selection started successfully, false otherwise</returns>
        bool StartSelection(Window owner);

        /// <summary>
        /// Gives focus to selection window that is on same screen as designated owner if a selection
        /// is started, otherwise does nothing
        /// </summary>
        /// <param name="owner">Main window that owns selection screens</param>
        void GiveSelectionFocus(Window owner);

        /// <summary>
        /// Enables all selection windows
        /// </summary>
        void EnableSelection();

        /// <summary>
        /// Disables all selection windows
        /// </summary>
        void DisableSelection();

        /// <summary>
        /// Exits selection operation
        /// </summary>
        void StopSelection();
    }
}
