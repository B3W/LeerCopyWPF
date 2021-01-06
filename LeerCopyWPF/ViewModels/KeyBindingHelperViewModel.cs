using LeerCopyWPF.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeerCopyWPF.ViewModels
{
    public class KeyBindingHelperViewModel : BaseViewModel
    {
        #region Fields
        #endregion // Fields

        #region Properties
        public override string DisplayName { get => "Leer Copy"; }

        public event EventHandler KeyBindingsChangedEvent;

        public IDictionary<string, string> KeyMappings { get; protected set; }
        #endregion // Properties

        #region Contructors
        public KeyBindingHelperViewModel()
        {
            KeyMappings = new Dictionary<string, string>(10);
        }
        #endregion // Contructors

        #region Methods
        public void RefreshKeyBinding(string key, string value)
        {
            if (SetMapping(key, value))
            {
                KeyBindingsChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        } // RefreshKeyBinding


        /// <summary>
        /// Fetches the key bindings from settings
        /// </summary>
        public void RefreshKeyBindings()
        {
            Properties.Settings settingsInst = Properties.Settings.Default;
            bool mappingsChanged = false;

            mappingsChanged |= SetMapping(SettingsConstants.CopySettingName, (string)settingsInst[SettingsConstants.CopySettingName]);
            mappingsChanged |= SetMapping(SettingsConstants.EditSettingName, (string)settingsInst[SettingsConstants.EditSettingName]);
            mappingsChanged |= SetMapping(SettingsConstants.SaveSettingName, (string)settingsInst[SettingsConstants.SaveSettingName]);
            mappingsChanged |= SetMapping(SettingsConstants.ClearSettingName, (string)settingsInst[SettingsConstants.ClearSettingName]);
            mappingsChanged |= SetMapping(SettingsConstants.SelectAllSettingName, (string)settingsInst[SettingsConstants.SelectAllSettingName]);
            mappingsChanged |= SetMapping(SettingsConstants.BorderSettingName, (string)settingsInst[SettingsConstants.BorderSettingName]);
            mappingsChanged |= SetMapping(SettingsConstants.TipsSettingName, (string)settingsInst[SettingsConstants.TipsSettingName]);
            mappingsChanged |= SetMapping(SettingsConstants.SwtchScrnSettingName, (string)settingsInst[SettingsConstants.SwtchScrnSettingName]);
            mappingsChanged |= SetMapping(SettingsConstants.SettingsSettingName, (string)settingsInst[SettingsConstants.SettingsSettingName]);
            mappingsChanged |= SetMapping(SettingsConstants.QuitSettingName, (string)settingsInst[SettingsConstants.QuitSettingName]);

            if (mappingsChanged)
            {
                KeyBindingsChangedEvent?.Invoke(this, EventArgs.Empty);
            }
        } // RefreshKeyBindings


        /// <summary>
        /// Updates key mapping to new value or adds new
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns>true if mapping changed, false otherwise</returns>
        private bool SetMapping(string key, string value)
        {
            bool mappingChanged = false;

            if (KeyMappings.TryGetValue(key, out string curValue))
            {
                if (value != curValue)
                {
                    KeyMappings[key] = value;
                    mappingChanged = true;
                }
            }
            else
            {
                KeyMappings.Add(key, value);
                mappingChanged = true;
            }

            return mappingChanged;
        } // SetMapping
        #endregion // Methods
    }
}
