﻿using LeerCopyWPF.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace LeerCopyWPF.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
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
        /// Keeps track of changes made to the settings with type string
        /// </summary>
        private IDictionary<string, string> _strSettingsChanges = new Dictionary<string, string>(12);
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
                    RecordStrPropertyChange(_copyPropName, value);
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
                    RecordStrPropertyChange(_editPropName, value);
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
                    RecordStrPropertyChange(_savePropName, value);
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
                    RecordStrPropertyChange(_clearPropName, value);
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
                    RecordStrPropertyChange(_selectAllPropName, value);
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
                    RecordStrPropertyChange(_borderPropName, value);
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
                    RecordStrPropertyChange(_tipsPropName, value);
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
                    RecordStrPropertyChange(_swtchScrnPropName, value);
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
                    RecordStrPropertyChange(_settingsPropName, value);
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
                    RecordStrPropertyChange(_quitPropName, value);
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
                    RecordStrPropertyChange(_defFileExtPropName, value);
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
                    RecordStrPropertyChange(_defFileNamePropName, value);
                }
            }
        }

        public bool CanSave
        {
            get
            {
                return _strSettingsChanges != null && _strSettingsChanges.Count > 0;
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(param => SaveSettings(), param => CanSave);
                }
                return _saveCommand;
            }
        }
        #endregion // Properties

        #region Constructors
        public SettingsViewModel()
        {
            _settingsInst = Properties.Settings.Default;

        }
        #endregion // Constructors

        #region Methods/Functions
        /// <summary>
        /// Adds or updates string type property change
        /// </summary>
        /// <param name="propertyName">Name of property changed</param>
        /// <param name="value">Value of property</param>
        private void RecordStrPropertyChange(string propertyName, string value)
        {
            // Adds new KeyValue pair if key doesn't exist, or updates existing
            _strSettingsChanges[propertyName] = value;
        }


        /// <summary>
        /// Logic for save command
        /// </summary>
        public void SaveSettings()
        {
            foreach (KeyValuePair<string, string> keyValuePair in _strSettingsChanges)
            {
                _settingsInst[keyValuePair.Key] = keyValuePair.Value;
            }
            _settingsInst.Save();
            OnPropertyChanged(_displayName);

            _strSettingsChanges.Clear();
        }
        #endregion // Methods/Functions
    }
}
