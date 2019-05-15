/*
 *  Leer Copy - Quick and Accurate Screen Capturing Application
 *  Copyright (C) 2019  Weston Berg
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program. If not, see <https://www.gnu.org/licenses/>.
 */

using LeerCopyWPF.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace LeerCopyWPF.ViewModels
{
    public class SettingsViewModel : BaseViewModel, IDataErrorInfo
    {
        #region Structs
        private struct SettingData
        {
            public readonly object data;
            public readonly Type type;

            public SettingData(object data, Type type)
            {
                this.data = data;
                this.type = type;
            }
        }
        #endregion

        #region Fields
        private const string _displayName = "Settings";
        /// <summary>
        /// Instance of application settings
        /// </summary>
        private Properties.Settings _settingsInst;
        /// <summary>
        /// Key binding
        /// </summary>
        private string _copy, _edit, _save, _clear, _selectAll, _border, _tips, _swtchScrn, _settings, _quit;
        /// <summary>
        /// Default file save setting
        /// </summary>
        private string _defaultExt, _defaultFileName;
        /// <summary>
        /// Setting for SelectionWindow background
        /// </summary>
        private double _selectWinOpacity;
        /// <summary>
        /// Visibility setting
        /// </summary>
        private Visibility _selectBorderVisibility, _tipsVisibility;
        /// <summary>
        /// Helper boolean for getting and setting visibility
        /// </summary>
        private bool _borderVisBool, _tipsVisBool;
        /// <summary>
        /// Set of options for default extension value
        /// </summary>
        private string[] _extOptions = new string[] { ".bmp", ".png", ".jpg", ".gif", ".tif", ".wmp" };
        /// <summary>
        /// Keeps track of changes made to the settings with type string
        /// </summary>
        private IDictionary<string, SettingData> _settingsChanges = new Dictionary<string, SettingData>(13);
        /// <summary>
        /// Command to run to save settings
        /// </summary>
        private ICommand _saveCommand;
        /// <summary>
        /// Helper flag for validating data
        /// </summary>
        private bool _fileNameValid = true, _fileExtValid = true, _opacityValid = true;
        #endregion // Fields

        #region Constants
        private const string ConstCopyPropName = "CopyKey";
        private const string ConstEditPropName = "EditKey";
        private const string ConstSavePropName = "SaveKey";
        private const string ConstClearPropName = "ClearKey";
        private const string ConstSelectAllPropName = "SelectAll";
        private const string ConstBorderPropName = "BorderKey";
        private const string ConstTipsPropName = "TipsKey";
        private const string ConstSwtchScrnPropName = "SwitchScreenKey";
        private const string ConstSettingsPropName = "SettingsWin";
        private const string ConstQuitPropName = "QuitKey";
        private const string ConstFileExtPropName = "DefaultSaveExt";
        private const string ConstFileNamePropName = "DefaultFileName";
        private const string ConstSelectWinOpacityPropName = "SelectionWinOpacity";
        private const string ConstSelectBorderVisPropName = "BorderVisibility";
        private const string ConstTipsVisPropName = "TipsVisibility";
        /// <summary>
        /// Max opacity for the selection window's background
        /// </summary>
        private const double ConstSelectOpacityMax = 0.3;
        #endregion // Constants

        #region Properties
        public override string DisplayName { get => _displayName; }

        public string CopyKey
        {
            get => _copy;
            set
            {
                if (_copy != value)
                {
                    _copy = value;
                    OnPropertyChanged(ConstCopyPropName);
                    RecordPropertyChange(ConstCopyPropName, value, typeof(string));
                }
            }
        }

        public string EditKey
        {
            get => _edit;
            set
            {
                if (_edit != value)
                {
                    _edit = value;
                    OnPropertyChanged(ConstEditPropName);
                    RecordPropertyChange(ConstEditPropName, value, typeof(string));
                }
            }
        }

        public string SaveKey
        {
            get => _save;
            set
            {
                if (_save != value)
                {
                    _save = value;
                    OnPropertyChanged(ConstSavePropName);
                    RecordPropertyChange(ConstSavePropName, value, typeof(string));
                }
            }
        }

        public string ClearKey
        {
            get => _clear;
            set
            {
                if (_clear != value)
                {
                    _clear = value;
                    OnPropertyChanged(ConstClearPropName);
                    RecordPropertyChange(ConstClearPropName, value, typeof(string));
                }
            }
        }

        public string SelectAll
        {
            get => _selectAll;
            set
            {
                if (_selectAll != value)
                {
                    _selectAll = value;
                    OnPropertyChanged(ConstSelectAllPropName);
                    RecordPropertyChange(ConstSelectAllPropName, value, typeof(string));
                }
            }
        }

        public string BorderKey
        {
            get => _border;
            set
            {
                if (_border != value)
                {
                    _border = value;
                    OnPropertyChanged(ConstBorderPropName);
                    RecordPropertyChange(ConstBorderPropName, value, typeof(string));
                }
            }
        }

        public string TipsKey
        {
            get => _tips;
            set
            {
                if (_tips != value)
                {
                    _tips = value;
                    OnPropertyChanged(ConstTipsPropName);
                    RecordPropertyChange(ConstTipsPropName, value, typeof(string));
                }
            }
        }

        public string SwitchScreenKey
        {
            get => _swtchScrn;
            set
            {
                if (_swtchScrn != value)
                {
                    _swtchScrn = value;
                    OnPropertyChanged(ConstSwtchScrnPropName);
                    RecordPropertyChange(ConstSwtchScrnPropName, value, typeof(string));
                }
            }
        }

        public string SettingsWin
        {
            get => _settings;
            set
            {
                if (_settings != value)
                {
                    _settings = value;
                    OnPropertyChanged(ConstSettingsPropName);
                    RecordPropertyChange(ConstSettingsPropName, value, typeof(string));
                }
            }
        }

        public string QuitKey
        {
            get => _quit;
            set
            {
                if (_quit != value)
                {
                    _quit = value;
                    OnPropertyChanged(ConstQuitPropName);
                    RecordPropertyChange(ConstQuitPropName, value, typeof(string));
                }
            }
        }

        public string DefaultSaveExt
        {
            get => _defaultExt;
            set
            {
                if (_defaultExt != value)
                {
                    _defaultExt = value;
                    OnPropertyChanged(ConstFileExtPropName);
                    RecordPropertyChange(ConstFileExtPropName, value, typeof(string));
                }
            }
        }

        public string DefaultFileName
        {
            get => _defaultFileName;
            set
            {
                if (_defaultFileName != value)
                {
                    _defaultFileName = value;
                    OnPropertyChanged(ConstFileNamePropName);
                    RecordPropertyChange(ConstFileNamePropName, value, typeof(string));
                }
            }
        }

        public double SelectionWinOpacity
        {
            get => _selectWinOpacity;
            set
            {
                if(_selectWinOpacity != value)
                {
                    _selectWinOpacity = value;
                    OnPropertyChanged(ConstSelectWinOpacityPropName);
                    RecordPropertyChange(ConstSelectWinOpacityPropName, value, typeof(double));
                }
            }
        }

        public bool BorderVisibility
        {
            get => _borderVisBool;
            set
            {
                if (value != _borderVisBool)
                {
                    _borderVisBool = value;
                    _selectBorderVisibility = (_borderVisBool == true) ? Visibility.Visible : Visibility.Hidden;
                    OnPropertyChanged(ConstSelectBorderVisPropName);
                    RecordPropertyChange(ConstSelectBorderVisPropName, _selectBorderVisibility, typeof(Visibility));
                }
            }
        }

        public bool TipsVisibility
        {
            get => _tipsVisBool;
            set
            {
                if (value != _tipsVisBool)
                {
                    _tipsVisBool = value;
                    _tipsVisibility = (_tipsVisBool == true) ? Visibility.Visible : Visibility.Hidden;
                    OnPropertyChanged(ConstTipsVisPropName);
                    RecordPropertyChange(ConstTipsVisPropName, _tipsVisibility, typeof(Visibility));
                }
            }
        }

        public string[] ExtOptions { get => _extOptions ?? (_extOptions = new string[] { ".bmp", ".png", ".jpg", ".gif", ".tif", ".wmp" }); }

        public double OpacityMax { get => ConstSelectOpacityMax; }

        public bool SettingsValid { get => _fileNameValid && _fileExtValid && _opacityValid; }

        public bool CanSave { get => SettingsValid && _settingsChanges != null && _settingsChanges.Count > 0; }

        public ICommand SaveCommand { get => _saveCommand ?? (_saveCommand = new RelayCommand(param => SaveSettings(), param => CanSave)); }

        public ICommand CloseCommand { get; }

        /// <summary>
        /// This property is not used by WPF framework so no implementation needed
        /// </summary>
        public string Error { get => throw new NotImplementedException(); }

        /// <summary>
        /// Validates given property data binding data
        /// </summary>
        /// <param name="columnName">Name of property to validate</param>
        /// <returns>null or empty string if valid, error message otherwise</returns>
        public string this[string columnName]
        { 
            get
            {
                string error = string.Empty;

                switch (columnName)
                {
                    case ConstCopyPropName:
                    case ConstEditPropName:
                    case ConstSavePropName:
                    case ConstClearPropName:
                    case ConstSelectAllPropName:
                    case ConstBorderPropName:
                    case ConstTipsPropName:
                    case ConstSwtchScrnPropName:
                    case ConstSettingsPropName:
                    case ConstQuitPropName:
                        // Validate key bindings
                        // TODO

                        break;
                    case ConstFileExtPropName:
                        // Validate file extension
                        if (Array.IndexOf<string>(ExtOptions, _defaultExt) == -1)
                        {
                            error = "File extension not supported";
                            _fileExtValid = false;
                        }
                        else
                        {
                            _fileExtValid = true;
                        }
                        break;
                    case ConstFileNamePropName:
                        // Validate file name
                        if (_defaultFileName.Length == 0 || _defaultFileName.Length > 100)
                        {
                            error = "Default file name must be between 1 and 100 characters long";
                            _fileNameValid = false;
                        }
                        else if (IsInvalidFileName(_defaultFileName))
                        {
                            error = "Invalid file name";
                            _fileNameValid = false;
                        }
                        else
                        {
                            _fileNameValid = true;
                        }
                        break;
                    case ConstSelectWinOpacityPropName:
                        // Validate selection window opacity value
                        if (_selectWinOpacity < 0.0 || _selectWinOpacity > ConstSelectOpacityMax)
                        {
                            error = "Selection window opacity must be between 0.0 and " + ConstSelectOpacityMax;
                            _opacityValid = false;
                        }
                        else
                        {
                            _opacityValid = true;
                        }
                        break;
                    default:
                        break;
                }

                return error;
            }
        }
        #endregion // Properties

        #region Constructors
        public SettingsViewModel(Action<object> closeAction)
        {
            _settingsInst = Properties.Settings.Default;

            // Fetch initial values from settings
            // Key bindings
            CopyKey = (string)_settingsInst[ConstCopyPropName];
            EditKey = (string)_settingsInst[ConstEditPropName];
            SaveKey = (string)_settingsInst[ConstSavePropName];
            ClearKey = (string)_settingsInst[ConstClearPropName];
            SelectAll = (string)_settingsInst[ConstSelectAllPropName];
            BorderKey = (string)_settingsInst[ConstBorderPropName];
            TipsKey = (string)_settingsInst[ConstTipsPropName];
            SwitchScreenKey = (string)_settingsInst[ConstSwtchScrnPropName];
            SettingsWin = (string)_settingsInst[ConstSettingsPropName];
            QuitKey = (string)_settingsInst[ConstQuitPropName];

            // Other
            DefaultSaveExt = (string)_settingsInst[ConstFileExtPropName];
            DefaultFileName = (string)_settingsInst[ConstFileNamePropName];
            SelectionWinOpacity = (double)_settingsInst[ConstSelectWinOpacityPropName];
            _selectBorderVisibility = (Visibility)_settingsInst[ConstSelectBorderVisPropName];
            _borderVisBool = (_selectBorderVisibility == Visibility.Visible) ? true : false;
            _tipsVisibility = (Visibility)_settingsInst[ConstTipsVisPropName];
            _tipsVisBool = (_tipsVisibility == Visibility.Visible) ? true : false;

            CloseCommand = new RelayCommand(closeAction);
        }
        #endregion // Constructors

        #region Methods/Functions
        /// <summary>
        /// Logic for save command
        /// </summary>
        public void SaveSettings()
        {
            if (!SettingsValid)
            {
                throw new InvalidOperationException("Trying to save invalid settings");
            }

            dynamic convertedSettingData;
            foreach (KeyValuePair<string, SettingData> keyValuePair in _settingsChanges)
            {
                convertedSettingData = Convert.ChangeType(keyValuePair.Value.data, keyValuePair.Value.type);
                _settingsInst[keyValuePair.Key] = convertedSettingData;
            }
            _settingsInst.Save();

            _settingsChanges.Clear();
        }


        /// <summary>
        /// Adds or updates string type property change
        /// </summary>
        /// <param name="propertyName">Name of property changed</param>
        /// <param name="value">Value of property</param>
        private void RecordPropertyChange(string propertyName, object value, Type type)
        {
            // Adds new KeyValue pair if key doesn't exist, or updates existing
            _settingsChanges[propertyName] = new SettingData(value, type);
        }


        /// <summary>
        /// Basic validation for the file name provided by the user
        /// </summary>
        /// <param name="filename">File name to validate</param>
        /// <returns>True if invalid, false otherwise</returns>
        private bool IsInvalidFileName(string filename)
        {
            string regexPattern = "^(COM[0-9]|CON|LPT[0-9]|NUL|PRN|AUX)$";
            Regex invalRegex = new Regex(regexPattern, RegexOptions.None);

            if (filename.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
            {
                return true;
            }
            else if (invalRegex.IsMatch(filename))
            {
                return true;
            }
            return false;
        }
        #endregion // Methods/Functions
    }
}
