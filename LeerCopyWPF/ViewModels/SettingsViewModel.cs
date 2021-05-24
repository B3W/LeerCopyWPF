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

using LeerCopyWPF.Commands;
using LeerCopyWPF.Constants;
using LeerCopyWPF.Enums;
using LeerCopyWPF.Models;
using Serilog;
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
        #region Fields

        /// <summary>
        /// Handle to logger for this source context
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// 'Copy selection' action key binding
        /// </summary>
        private readonly Setting<string> _copyKeyBinding;

        /// <summary>
        /// 'Edit selection' action key binding
        /// </summary>
        private readonly Setting<string> _editKeyBinding;

        /// <summary>
        /// 'Save selection' action key binding
        /// </summary>
        private readonly Setting<string> _saveKeyBinding;

        /// <summary>
        /// 'Clear selection' action key binding
        /// </summary>
        private readonly Setting<string> _clearKeyBinding;

        /// <summary>
        /// 'Select all' action key binding
        /// </summary>
        private readonly Setting<string> _selectAllKeyBinding;

        /// <summary>
        /// 'Toggle border' action key binding
        /// </summary>
        private readonly Setting<string> _toggleBorderKeyBinding;

        /// <summary>
        /// 'Toggle tips' action key binding
        /// </summary>
        private readonly Setting<string> _toggleTipsKeyBinding;

        /// <summary>
        /// 'Open settings' action key binding
        /// </summary>
        private readonly Setting<string> _settingsKeyBinding;

        /// <summary>
        /// 'Quit selection' action key binding
        /// </summary>
        private readonly Setting<string> _quitKeyBinding;

        /// <summary>
        /// Default file extension used when saving selections
        /// </summary>
        private readonly Setting<string> _defaultFileExt;

        /// <summary>
        /// Default file name used when saving selections
        /// </summary>
        private readonly Setting<string> _defaultFileName;

        /// <summary>
        /// Selection window background opacity
        /// </summary>
        private readonly Setting<double> _selectionOpacity;

        /// <summary>
        /// Visibility of selection border
        /// </summary>
        private readonly Setting<bool> _selectionBorderVisibility;

        /// <summary>
        /// Visibility of tips/hints
        /// </summary>
        private readonly Setting<bool> _tipsVisibility;

        /// <summary>
        /// Set of options for default extension value
        /// </summary>
        private readonly string[] _extOptions = new string[] { ".bmp", ".png", ".jpg", ".gif", ".tif", ".wmp" };

        /// <summary>
        /// List of key bindings (these settings are coupled together)
        /// </summary>
        private readonly IList<ISetting> _keyBindings;

        /// <summary>
        /// List of general settings (these settings do not depend on each other)
        /// </summary>
        private readonly IList<ISetting> _generalSettings;

        /// <summary>
        /// Current validation errors (Mapping => Setting ID : Error Message)
        /// </summary>
        private readonly IDictionary<string, string> _validationErrors;

        #endregion // Fields


        #region Properties

        public override string DisplayName { get => "Settings"; }

        /// <summary>
        /// Bound to 'Copy' key binding textbox
        /// </summary>
        public string CopyKey
        {
            get => _copyKeyBinding.CurrentValue;
            set
            {
                if (_copyKeyBinding.CurrentValue != value)
                {
                    _copyKeyBinding.CurrentValue = value;
                    ValidateCoupledSettings(_keyBindings);
                    OnPropertyChanged(_copyKeyBinding.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to 'Edit' key binding textbox
        /// </summary>
        public string EditKey
        {
            get => _editKeyBinding.CurrentValue;
            set
            {
                if (_editKeyBinding.CurrentValue != value)
                {
                    _editKeyBinding.CurrentValue = value;
                    ValidateCoupledSettings(_keyBindings);
                    OnPropertyChanged(_editKeyBinding.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to 'Save' key binding textbox
        /// </summary>
        public string SaveKey
        {
            get => _saveKeyBinding.CurrentValue;
            set
            {
                if (_saveKeyBinding.CurrentValue != value)
                {
                    _saveKeyBinding.CurrentValue = value;
                    ValidateCoupledSettings(_keyBindings);
                    OnPropertyChanged(_saveKeyBinding.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to 'Clear' key binding textbox
        /// </summary>
        public string ClearKey
        {
            get => _clearKeyBinding.CurrentValue;
            set
            {
                if (_clearKeyBinding.CurrentValue != value)
                {
                    _clearKeyBinding.CurrentValue = value;
                    ValidateCoupledSettings(_keyBindings);
                    OnPropertyChanged(_clearKeyBinding.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to 'Select All' key binding textbox
        /// </summary>
        public string SelectAll
        {
            get => _selectAllKeyBinding.CurrentValue;
            set
            {
                if (_selectAllKeyBinding.CurrentValue != value)
                {
                    _selectAllKeyBinding.CurrentValue = value;
                    ValidateCoupledSettings(_keyBindings);
                    OnPropertyChanged(_selectAllKeyBinding.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to 'Toggle Border' key binding textbox
        /// </summary>
        public string BorderKey
        {
            get => _toggleBorderKeyBinding.CurrentValue;
            set
            {
                if (_toggleBorderKeyBinding.CurrentValue != value)
                {
                    _toggleBorderKeyBinding.CurrentValue = value;
                    ValidateCoupledSettings(_keyBindings);
                    OnPropertyChanged(_toggleBorderKeyBinding.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to 'Toggle Tips' key binding textbox
        /// </summary>
        public string TipsKey
        {
            get => _toggleTipsKeyBinding.CurrentValue;
            set
            {
                if (_toggleTipsKeyBinding.CurrentValue != value)
                {
                    _toggleTipsKeyBinding.CurrentValue = value;
                    ValidateCoupledSettings(_keyBindings);
                    OnPropertyChanged(_toggleTipsKeyBinding.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to 'Open Settings' key binding textbox
        /// </summary>
        public string SettingsWin
        {
            get => _settingsKeyBinding.CurrentValue;
            set
            {
                if (_settingsKeyBinding.CurrentValue != value)
                {
                    _settingsKeyBinding.CurrentValue = value;
                    ValidateCoupledSettings(_keyBindings);
                    OnPropertyChanged(_settingsKeyBinding.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to 'Quit' key binding textbox
        /// </summary>
        public string QuitKey
        {
            get => _quitKeyBinding.CurrentValue;
            set
            {
                if (_quitKeyBinding.CurrentValue != value)
                {
                    _quitKeyBinding.CurrentValue = value;
                    ValidateCoupledSettings(_keyBindings);
                    OnPropertyChanged(_quitKeyBinding.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to file extension dropdown
        /// </summary>
        public string DefaultSaveExt
        {
            get => _defaultFileExt.CurrentValue;
            set
            {
                if (_defaultFileExt.CurrentValue != value)
                {
                    _defaultFileExt.CurrentValue = value;
                    ValidateDiscreteSetting(_defaultFileExt);
                    OnPropertyChanged(_defaultFileExt.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to file name textbox
        /// </summary>
        public string DefaultFileName
        {
            get => _defaultFileName.CurrentValue;
            set
            {
                if (_defaultFileName.CurrentValue != value)
                {
                    _defaultFileName.CurrentValue = value;
                    ValidateDiscreteSetting(_defaultFileName);
                    OnPropertyChanged(_defaultFileName.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to opacity slider
        /// </summary>
        public double SelectionWinOpacity
        {
            get => _selectionOpacity.CurrentValue;
            set
            {
                if(_selectionOpacity.CurrentValue != value)
                {
                    _selectionOpacity.CurrentValue = value;
                    ValidateDiscreteSetting(_selectionOpacity);
                    OnPropertyChanged(_selectionOpacity.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to border visibility checkbox
        /// </summary>
        public bool BorderVisibility
        {
            get => _selectionBorderVisibility.CurrentValue;
            set
            {
                if (_selectionBorderVisibility.CurrentValue != value)
                {
                    _selectionBorderVisibility.CurrentValue = value;
                    ValidateDiscreteSetting(_selectionBorderVisibility);
                    OnPropertyChanged(_selectionBorderVisibility.SettingID);
                }
            }
        }

        /// <summary>
        /// Bound to tips visibility checkbox
        /// </summary>
        public bool TipsVisibility
        {
            get => _tipsVisibility.CurrentValue;
            set
            {
                if (_tipsVisibility.CurrentValue != value)
                {
                    _tipsVisibility.CurrentValue = value;
                    ValidateDiscreteSetting(_tipsVisibility);
                    OnPropertyChanged(_tipsVisibility.SettingID);
                }
            }
        }

        /// <summary>
        /// File extension options for the dropdown menu
        /// </summary>
        public string[] ExtOptions { get => _extOptions; }

        /// <summary>
        /// Max value for selection window opacity
        /// </summary>
        public double OpacityMax { get => SettingsConstants.ConstSelectOpacityMax; }

        /// <summary>
        /// Flag indicating if settings are valid
        /// </summary>
        public bool SettingsValid { get => _validationErrors.Count == 0; }

        /// <summary>
        /// Flag indicating if the settings have been modified
        /// </summary>
        public bool SettingsChanged { get => SettingsContainChanges(); }

        /// <summary>
        /// Flag indicating if settings can be saved
        /// </summary>
        public bool CanSave { get => SettingsValid && SettingsChanged; }

        /// <summary>
        /// Command for saving settings
        /// </summary>
        public ICommand SaveCommand { get; }

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
                string error;

                if (_validationErrors.ContainsKey(columnName))
                {
                    error = _validationErrors[columnName];
                }
                else
                {
                    error = string.Empty;
                }

                return error;
            }
        }

        #endregion // Properties


        #region Methods

        /// <summary>
        /// Constructs instance of SettingsViewModel
        /// </summary>
        public SettingsViewModel()
        {
            _logger = Log.ForContext<SettingsViewModel>();
            _keyBindings = new List<ISetting>();
            _generalSettings = new List<ISetting>();
            _validationErrors = new Dictionary<string, string>();

            // Load 'Copy Selection' key binding
            _copyKeyBinding = new Setting<string>(SettingsConstants.CopySettingName,
                                                  Properties.Settings.Default.CopyKey,
                                                  ValidateKeyBinding);
            _keyBindings.Add(_copyKeyBinding);

            // Load 'Edit Selection' key binding
            _editKeyBinding = new Setting<string>(SettingsConstants.EditSettingName,
                                                  Properties.Settings.Default.EditKey,
                                                  ValidateKeyBinding);
            _keyBindings.Add(_editKeyBinding);

            // Load 'Save Selection' key binding
            _saveKeyBinding = new Setting<string>(SettingsConstants.SaveSettingName,
                                                  Properties.Settings.Default.SaveKey,
                                                  ValidateKeyBinding);
            _keyBindings.Add(_saveKeyBinding);

            // Load 'Clear Selection' key binding
            _clearKeyBinding = new Setting<string>(SettingsConstants.ClearSettingName,
                                                   Properties.Settings.Default.ClearKey,
                                                   ValidateKeyBinding);
            _keyBindings.Add(_clearKeyBinding);

            // Load 'Select All' key binding
            _selectAllKeyBinding = new Setting<string>(SettingsConstants.SelectAllSettingName,
                                                       Properties.Settings.Default.SelectAll,
                                                       ValidateKeyBinding);
            _keyBindings.Add(_selectAllKeyBinding);

            // Load 'Border' key binding
            _toggleBorderKeyBinding = new Setting<string>(SettingsConstants.BorderSettingName,
                                                          Properties.Settings.Default.BorderKey,
                                                          ValidateKeyBinding);
            _keyBindings.Add(_toggleBorderKeyBinding);

            // Load 'Tips' key binding
            _toggleTipsKeyBinding = new Setting<string>(SettingsConstants.TipsSettingName,
                                                        Properties.Settings.Default.TipsKey,
                                                        ValidateKeyBinding);
            _keyBindings.Add(_toggleTipsKeyBinding);

            // Load 'Settings' key binding
            _settingsKeyBinding = new Setting<string>(SettingsConstants.SettingsSettingName,
                                                      Properties.Settings.Default.SettingsWin,
                                                      ValidateKeyBinding);
            _keyBindings.Add(_settingsKeyBinding);

            // Load 'Quit Selection' key binding
            _quitKeyBinding = new Setting<string>(SettingsConstants.QuitSettingName,
                                                  Properties.Settings.Default.QuitKey,
                                                  ValidateKeyBinding);
            _keyBindings.Add(_quitKeyBinding);

            // Load default file extension setting
            _defaultFileExt = new Setting<string>(SettingsConstants.ConstFileExtPropName,
                                                  Properties.Settings.Default.DefaultSaveExt,
                                                  ValidateFileExtension);
            _generalSettings.Add(_defaultFileExt);

            // Load default file name setting
            _defaultFileName = new Setting<string>(SettingsConstants.ConstFileNamePropName,
                                                   Properties.Settings.Default.DefaultFileName,
                                                   ValidateFileName);
            _generalSettings.Add(_defaultFileName);

            // Load selection window opacity setting
            _selectionOpacity = new Setting<double>(SettingsConstants.ConstSelectWinOpacityPropName,
                                                    Properties.Settings.Default.SelectionWinOpacity,
                                                    ValidateOpacity);
            _generalSettings.Add(_selectionOpacity);

            // Load border visibility setting
            _selectionBorderVisibility = new Setting<bool>(SettingsConstants.ConstSelectBorderVisPropName, Properties.Settings.Default.BorderVisibility);
            _generalSettings.Add(_selectionBorderVisibility);

            // Load tips visibility setting
            _tipsVisibility = new Setting<bool>(SettingsConstants.ConstTipsVisPropName, Properties.Settings.Default.TipsVisibility);
            _generalSettings.Add(_tipsVisibility);

            SaveCommand = new RelayCommand(param => SaveSettings(), param => CanSave);
        }


        /// <summary>
        /// Checks if settings were modified
        /// </summary>
        /// <returns>true if settings modified, false otherwise</returns>
        private bool SettingsContainChanges()
        {
            bool settingsModified = false;

            // Check key bindings for changes
            foreach (ISetting binding in _keyBindings)
            {
                if (binding.ValueModified)
                {
                    settingsModified = true;
                    break;
                }
            }

            // Check general settings for changes
            if (!settingsModified)
            {
                foreach (ISetting setting in _generalSettings)
                {
                    if (setting.ValueModified)
                    {
                        settingsModified = true;
                        break;
                    }
                }
            }

            return settingsModified;
        }


        /// <summary>
        /// Logic for save command
        /// </summary>
        private void SaveSettings()
        {
            if (!SettingsValid)
            {
                _logger.Fatal("Tried to save invalid settings");
                throw new InvalidOperationException("Trying to save invalid settings");
            }

            _logger.Debug("User requested to save settings");

            // Save key bindings
            foreach (ISetting keyBinding in _keyBindings)
            {
                keyBinding.Save();
            }

            // Save the general settings
            foreach (ISetting setting in _generalSettings)
            {
                setting.Save();
            }
        }


        /// <summary>
        /// Validates entire set of coupled settings
        /// </summary>
        /// <param name="coupledSettings">List of coupled settings to validate</param>
        /// <returns>true if valid, false otherwise</returns>
        private bool ValidateCoupledSettings(IList<ISetting> coupledSettings)
        {
            bool coupledSettingsValid = true;

            // Re-evaluate validation on all coupled settings
            foreach (ISetting setting in coupledSettings)
            {
                bool settingValid = ValidateDiscreteSetting(setting);

                if (settingValid)
                {
                    // Clear UI's error indication if it exists
                    OnPropertyChanged(setting.SettingID);
                }

                coupledSettingsValid &= settingValid;
            }

            return coupledSettingsValid;
        }


        /// <summary>
        /// Validates a setting independent of all other settings
        /// </summary>
        /// <param name="setting">Setting to validate</param>
        /// <returns>true if valid, false otherwise</returns>
        private bool ValidateDiscreteSetting(ISetting setting)
        {
            bool isValid = setting.Validate();

            if (isValid)
            {
                // Remove validation error if it exists
                if (_validationErrors.ContainsKey(setting.SettingID))
                {
                    _validationErrors.Remove(setting.SettingID);
                }
            }
            else
            {
                // Add error or replace existing error
                _validationErrors[setting.SettingID] = setting.ErrorMessage;
            }

            return isValid;
        }


        /// <summary>
        /// Validates key binding
        /// </summary>
        /// <param name="binding">Binding to valdiate</param>
        /// <returns>true if valid, false otherwise</returns>
        private bool ValidateKeyBinding(Setting<string> binding)
        {
            bool isValid = true;
            binding.ErrorMessage = string.Empty;

            foreach (Setting<string> activeBinding in _keyBindings)
            {
                // Skip checking binding against itself
                if (binding.SettingID != activeBinding.SettingID)
                {
                    // Check if key is already bound
                    if (binding.CurrentValue == activeBinding.CurrentValue)
                    {
                        isValid = false;
                        binding.ErrorMessage = binding.CurrentValue + " is already bound";
                        break;
                    }
                }
            }

            return isValid;
        }


        /// <summary>
        /// Validates the default file extension setting
        /// </summary>
        /// <param name="setting">Setting to validate</param>
        /// <returns>true if valid, false otherwise</returns>
        private bool ValidateFileExtension(Setting<string> setting)
        {
            bool isValid;

            if (Array.IndexOf(ExtOptions, setting.CurrentValue) == -1)
            {
                isValid = false;
                setting.ErrorMessage = "File extension not supported";
            }
            else
            {
                isValid = true;
                setting.ErrorMessage = string.Empty;
            }

            return isValid;
        }


        /// <summary>
        /// Validates the default file name setting
        /// </summary>
        /// <param name="setting">Setting to validate</param>
        /// <returns>true if valid, false otherwise</returns>
        private bool ValidateFileName(Setting<string> setting)
        {
            bool isValid;

            if (setting.CurrentValue.Length == 0 || setting.CurrentValue.Length > 100)
            {
                isValid = false;
                setting.ErrorMessage = "Default file name must be between 1 and 100 characters long";
            }
            else if (IsInvalidFileName(setting.CurrentValue))
            {
                isValid = false;
                setting.ErrorMessage = "Invalid file name";
            }
            else
            {
                isValid = true;
                setting.ErrorMessage = string.Empty;
            }

            return isValid;
        }


        /// <summary>
        /// Basic validation for the file name provided by the user
        /// </summary>
        /// <param name="filename">File name to validate</param>
        /// <returns>True if invalid, false otherwise</returns>
        private bool IsInvalidFileName(string filename)
        {
            const string regexPattern = "^(COM[0-9]|CON|LPT[0-9]|NUL|PRN|AUX)$";
            Regex invalidNameRegex = new Regex(regexPattern, RegexOptions.None);
            bool isInvalid = false;

            if (filename.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1)
            {
                isInvalid = true;
            }

            if (invalidNameRegex.IsMatch(filename))
            {
                isInvalid = true;
            }

            return isInvalid;
        }


        /// <summary>
        /// Validates selection window opacity setting
        /// </summary>
        /// <param name="setting">Setting to validate</param>
        /// <returns>true if valid, false otherwise</returns>
        private bool ValidateOpacity(Setting<double> setting)
        {
            bool opacityValue;

            if (setting.CurrentValue < 0.0 || setting.CurrentValue > OpacityMax)
            {
                opacityValue = false;
                setting.ErrorMessage = "Selection window opacity must be between 0.0 and " + OpacityMax;
            }
            else
            {
                opacityValue = true;
                setting.ErrorMessage = string.Empty;
            }

            return opacityValue;
        }

        #endregion // Methods
    }
}
