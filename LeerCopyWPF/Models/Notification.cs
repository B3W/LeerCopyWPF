using LeerCopyWPF.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
