using LeerCopyWPF.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeerCopyWPF.ViewModels
{
    public class KeyBindingHelperViewModel : BaseViewModel
    {
        #region Fields
        protected bool _mappingsChanged;
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
            SetMapping(key, value);

            if (_mappingsChanged)
            {
                if (KeyBindingsChangedEvent != null)
                {
                    KeyBindingsChangedEvent.Invoke(this, EventArgs.Empty);
                    _mappingsChanged = false;
                }
            }
        } // RefreshKeyBinding


        /// <summary>
        /// Fetches the key bindings from settings
        /// </summary>
        public void RefreshKeyBindings()
        {
            Properties.Settings settingsInst = Properties.Settings.Default;

            SetMapping(SettingsConstants.CopySettingName, (string)settingsInst[SettingsConstants.CopySettingName]);
            SetMapping(SettingsConstants.EditSettingName, (string)settingsInst[SettingsConstants.EditSettingName]);
            SetMapping(SettingsConstants.SaveSettingName, (string)settingsInst[SettingsConstants.SaveSettingName]);
            SetMapping(SettingsConstants.ClearSettingName, (string)settingsInst[SettingsConstants.ClearSettingName]);
            SetMapping(SettingsConstants.SelectAllSettingName, (string)settingsInst[SettingsConstants.SelectAllSettingName]);
            SetMapping(SettingsConstants.BorderSettingName, (string)settingsInst[SettingsConstants.BorderSettingName]);
            SetMapping(SettingsConstants.TipsSettingName, (string)settingsInst[SettingsConstants.TipsSettingName]);
            SetMapping(SettingsConstants.SwtchScrnSettingName, (string)settingsInst[SettingsConstants.SwtchScrnSettingName]);
            SetMapping(SettingsConstants.SettingsSettingName, (string)settingsInst[SettingsConstants.SettingsSettingName]);
            SetMapping(SettingsConstants.QuitSettingName, (string)settingsInst[SettingsConstants.QuitSettingName]);

            if (_mappingsChanged)
            {
                if (KeyBindingsChangedEvent != null)
                {
                    KeyBindingsChangedEvent.Invoke(this, EventArgs.Empty);
                    _mappingsChanged = false;
                }
            }
        } // RefreshKeyBindings


        /// <summary>
        /// Updates key mapping to new value or adds new
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void SetMapping(string key, string value)
        {
            if (KeyMappings.TryGetValue(key, out string curValue))
            {
                if (value != curValue)
                {
                    KeyMappings[key] = value;
                    _mappingsChanged = true;
                }
            }
            else
            {
                KeyMappings.Add(key, value);
                _mappingsChanged = true;
            }
        } // SetMapping
        #endregion // Methods
    }
}
