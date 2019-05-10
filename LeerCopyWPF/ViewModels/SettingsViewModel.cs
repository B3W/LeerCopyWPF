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
using LeerCopyWPF.Utilities;
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
        private string[] _extOptions;
        /// <summary>
        /// Keeps track of changes made to the settings with type string
        /// </summary>
        private IDictionary<string, SettingData> _settingsChanges = new Dictionary<string, SettingData>(13);
        /// <summary>
        /// Command to run to save settings
        /// </summary>
        private ICommand _saveCommand;
        #endregion // Fields

        #region PropertyNames
        private const string _copyPropName = "CopyKey";
        private const string _editPropName = "EditKey";
        private const string _savePropName = "SaveKey";
        private const string _clearPropName = "ClearKey";
        private const string _selectAllPropName = "SelectAll";
        private const string _borderPropName = "BorderKey";
        private const string _tipsPropName = "TipsKey";
        private const string _swtchScrnPropName = "SwitchScreenKey";
        private const string _settingsPropName = "SettingsWin";
        private const string _quitPropName = "QuitKey";
        private const string _defFileExtPropName = "DefaultSaveExt";
        private const string _defFileNamePropName = "DefaultFileName";
        private const string _selectWinOpacityPropName = "SelectionWinOpacity";
        private const string _selectBorderVisPropName = "BorderVisibility";
        private const string _tipsVisPropName = "TipsVisibility";
        #endregion // PropertyNames

        #region Properties
        public override string DisplayName
        {
            get => _displayName;
        }

        public string CopyKey
        {
            get => _copy;
            set
            {
                if (_copy != value)
                {
                    _copy = value;
                    OnPropertyChanged(_copyPropName);
                    RecordPropertyChange(_copyPropName, value, typeof(string));
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
                    OnPropertyChanged(_editPropName);
                    RecordPropertyChange(_editPropName, value, typeof(string));
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
                    OnPropertyChanged(_savePropName);
                    RecordPropertyChange(_savePropName, value, typeof(string));
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
                    OnPropertyChanged(_clearPropName);
                    RecordPropertyChange(_clearPropName, value, typeof(string));
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
                    OnPropertyChanged(_selectAllPropName);
                    RecordPropertyChange(_selectAllPropName, value, typeof(string));
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
                    OnPropertyChanged(_borderPropName);
                    RecordPropertyChange(_borderPropName, value, typeof(string));
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
                    OnPropertyChanged(_tipsPropName);
                    RecordPropertyChange(_tipsPropName, value, typeof(string));
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
                    OnPropertyChanged(_swtchScrnPropName);
                    RecordPropertyChange(_swtchScrnPropName, value, typeof(string));
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
                    OnPropertyChanged(_settingsPropName);
                    RecordPropertyChange(_settingsPropName, value, typeof(string));
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
                    OnPropertyChanged(_quitPropName);
                    RecordPropertyChange(_quitPropName, value, typeof(string));
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
                    OnPropertyChanged(_defFileExtPropName);
                    RecordPropertyChange(_defFileExtPropName, value, typeof(string));
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
                    OnPropertyChanged(_defFileNamePropName);
                    RecordPropertyChange(_defFileNamePropName, value, typeof(string));
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
                    OnPropertyChanged(_selectWinOpacityPropName);
                    RecordPropertyChange(_selectWinOpacityPropName, value, typeof(double));
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
                    OnPropertyChanged(_selectBorderVisPropName);
                    RecordPropertyChange(_selectBorderVisPropName, _selectBorderVisibility, typeof(Visibility));
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
                    OnPropertyChanged(_tipsVisPropName);
                    RecordPropertyChange(_tipsVisPropName, _tipsVisibility, typeof(Visibility));
                }
            }
        }

        public string[] ExtOptions
        {
            get
            {
                if (_extOptions == null)
                {
                    _extOptions = new string[] { ".bmp", ".png", ".jpg", ".gif", ".tif", ".wmp" };
                }
                return _extOptions;
            }
        }

        /// <summary>
        /// Allow saving only if changes have been made
        /// </summary>
        public bool CanSave
        {
            get
            {
                return _settingsChanges != null && _settingsChanges.Count > 0;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new RelayCommand(param => SaveSettings(), param => CanSave));
            }
        }

        public ICommand CloseCommand { get; }

        /// <summary>
        /// This property is not used by WPF framework so no implementation needed
        /// </summary>
        public string Error
        {
            get => throw new NotImplementedException();
        }

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
                    case _copyPropName:
                    case _editPropName:
                    case _savePropName:
                    case _clearPropName:
                    case _selectAllPropName:
                    case _borderPropName:
                    case _tipsPropName:
                    case _swtchScrnPropName:
                    case _settingsPropName:
                    case _quitPropName:
                        // Validate key bindings
                        // TODO

                        break;
                    case _defFileExtPropName:
                        // Validate file extension
                        if (Array.IndexOf<string>(_extOptions, _defaultExt) == -1)
                        {
                            error = "File extension not supported";
                        }
                        break;
                    case _defFileNamePropName:
                        // Validate file name
                        if (_defaultFileName.Length == 0 || _defaultFileName.Length > 100)
                        {
                            error = "Default file name must be between 1 and 100 characters long";
                        }
                        else if (IsInvalidFileName(_defaultFileName))
                        {
                            error = "Invalid file name";
                        }
                        break;
                    case _selectWinOpacityPropName:
                        // Validate selection window opacity value
                        if (_selectWinOpacity < 0.0 || _selectWinOpacity > 0.25)
                        {
                            error = "Selection Window Opacity must be between 0.0 and 0.25";
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
            _copy = (string)_settingsInst[_copyPropName];
            _edit = (string)_settingsInst[_editPropName];
            _save = (string)_settingsInst[_savePropName];
            _clear = (string)_settingsInst[_clearPropName];
            _selectAll = (string)_settingsInst[_selectAllPropName];
            _border = (string)_settingsInst[_borderPropName];
            _tips = (string)_settingsInst[_tipsPropName];
            _swtchScrn = (string)_settingsInst[_swtchScrnPropName];
            _settings = (string)_settingsInst[_settingsPropName];
            _quit = (string)_settingsInst[_quitPropName];
            _defaultExt = (string)_settingsInst[_defFileExtPropName];
            _defaultFileName = (string)_settingsInst[_defFileNamePropName];
            _selectWinOpacity = (double)_settingsInst[_selectWinOpacityPropName];
            _selectBorderVisibility = (Visibility)_settingsInst[_selectBorderVisPropName];
            _borderVisBool = (_selectBorderVisibility == Visibility.Visible) ? true : false;
            _tipsVisibility = (Visibility)_settingsInst[_tipsVisPropName];
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
        /// Validates the file name provided by the user
        /// </summary>
        /// <param name="filename">File name to validate</param>
        /// <returns>True if invalid, false otherwise</returns>
        private bool IsInvalidFileName(string filename)
        {
            string regexPattern = "^(?!^(?:PRN|AUX|CLOCK\\$|NUL|CON|COM\\d|LPT\\d)(?:\\..+)?$)(?:\\.*?(?!\\.))[^\x00-\x1f\\?*:\";|\\/<>]+(?<![\\s.])$";
            Regex invalRegex = new Regex(regexPattern, RegexOptions.IgnoreCase);

            if (invalRegex.IsMatch(filename))
            {
                return true;
            }
            return false;
        }
        #endregion // Methods/Functions
    }
}
