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

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Methods

        #region Public Methods

        /// <summary>
        /// Constructs Notification instance
        /// </summary>
        /// <param name="title">Title for the notification</param>
        /// <param name="message">Message to be displayed for the notification</param>
        /// <param name="notificationType">Type of notification displayed</param>
        public Notification(string title, string message, NotificationType notificationType)
        {
            Title = title ?? ConstDefaultTitle;
            Message = message;
            NotificationType = notificationType;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion // Methods
    }
}
