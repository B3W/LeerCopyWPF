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
