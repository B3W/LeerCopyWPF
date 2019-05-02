using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LeerCopyWPF.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        #region Properties
        public abstract string DisplayName { get; }
        #endregion //Properties

        #region INotifyPropertyChanged Member
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion // INotifyPropertyChanged Member

        #region Methods
        /// <summary>
        /// Called when a property value changes
        /// </summary>
        /// <param name="propertyName">Name of property which changed</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            VerifyPropertyName(propertyName);
            // Make copy of handler to avoid thread issues
            PropertyChangedEventHandler handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Verifies the given property name corresponds to valid property
        /// </summary>
        /// <param name="propertyName">Name of property to verify</param>
        protected void VerifyPropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                string errorMsg = "ViewModel does not contain property: " + propertyName;
                throw new ArgumentException("propertyName", errorMsg);
            }
        }
        #endregion
    }
}
