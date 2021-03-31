using LeerCopyWPF.Enums;
using LeerCopyWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
