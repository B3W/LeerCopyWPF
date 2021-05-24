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

using LeerCopyWPF.Enums;
using LeerCopyWPF.Models;

namespace LeerCopyWPF.ViewModels
{
    public class NotificationViewModel : BaseViewModel
    {
        #region Fields

        #region Public Fields
        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields

        /// <summary>
        /// Handle to underlying notification model
        /// </summary>
        private readonly Notification _notification;

        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties

        public override string DisplayName { get => _notification.Title; }

        /// <summary>
        /// Message to be displayed for the notification
        /// </summary>
        public string Message { get => _notification.Message; }

        /// <summary>
        /// Type of notification displayed
        /// </summary>
        public NotificationType NotificationType { get => _notification.NotificationType; }

        /// <summary>
        /// User options for notification
        /// </summary>
        public NotificationOptions UserOptions { get => _notification.UserOptions; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructs NotificationViewModel instance
        /// </summary>
        /// <param name="notification">Underlying notification data</param>
        public NotificationViewModel(Notification notification)
        {
            _notification = notification;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
