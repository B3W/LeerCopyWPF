using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeerCopyWPF.Models
{
    public class Setting<T> : ISetting where T : IEquatable<T>
    {
        #region Fields

        #region Public Fields
        #endregion

        #region Protected Fields
        #endregion

        #region Private Fields

        private readonly Predicate<Setting<T>> _validationFunc;

        #endregion

        #endregion // Fields


        #region Properties

        #region Public Properties

        public T OriginalValue { get; private set; }

        public T CurrentValue { get; set; }

        public string SettingID { get; }

        public bool ValueModified { get => !OriginalValue.Equals(CurrentValue); }

        public bool IsValid { get; private set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Protected Properties
        #endregion

        #region Private Properties
        #endregion

        #endregion // Properties


        #region Fields

        #region Public Methods

        public Setting(string settingId, T originalValue) : this(settingId, originalValue, null) { }

        public Setting(string settingId, T originalValue, Predicate<Setting<T>> validationFunc)
        {
            _validationFunc = validationFunc;

            SettingID = settingId;
            OriginalValue = originalValue;
            CurrentValue = originalValue;
        }

        public bool Validate()
        {
            bool isValid = true;

            if (_validationFunc != null)
            {
                isValid = _validationFunc(this);
            }

            IsValid = isValid;

            return isValid;
        }

        public bool Save()
        {
            Properties.Settings.Default[SettingID] = CurrentValue;
            Properties.Settings.Default.Save();

            OriginalValue = CurrentValue;

            return true;
        }

        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods

        #endregion

        #endregion // Methods
    }
}
