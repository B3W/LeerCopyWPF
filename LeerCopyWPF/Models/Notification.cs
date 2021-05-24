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

namespace LeerCopyWPF.Models
{
    public class Notification
    {
        #region Constants

        private const string ConstDefaultTitle = "Notification";

        #endregion // Constants


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
        /// Title for the notification
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Message to be displayed for the notification
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Type of notification displayed
        /// </summary>
        public NotificationType NotificationType { get; }

        /// <summary>
        /// User options for notification
        /// </summary>
        public NotificationOptions UserOptions { get; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructs Notification instance with notification type set to 'Information' and user options set to 'Ok'
        /// </summary>
        /// <param name="title">Title for the notification</param>
        /// <param name="message">Message to be displayed for the notification</param>
        public Notification(string title, string message) : this(title, message, NotificationType.Information, NotificationOptions.Ok) { }


        /// <summary>
        /// Constructs Notification instance with user options set to 'Ok'
        /// </summary>
        /// <param name="title">Title for the notification</param>
        /// <param name="message">Message to be displayed for the notification</param>
        /// <param name="notificationType">Type of notification displayed</param>
        public Notification(string title, string message, NotificationType notificationType) : this(title, message, notificationType, NotificationOptions.Ok) { }


        /// <summary>
        /// Constructs Notification instance
        /// </summary>
        /// <param name="title">Title for the notification</param>
        /// <param name="message">Message to be displayed for the notification</param>
        /// <param name="notificationType">Type of notification displayed</param>
        /// <param name="notificationOptions">User options for notification</param>
        public Notification(string title, string message, NotificationType notificationType, NotificationOptions notificationOptions)
        {
            Title = title ?? ConstDefaultTitle;
            Message = message;
            NotificationType = notificationType;
            UserOptions = notificationOptions;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
