using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LeerCopyWPF.ViewModels
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Fields
        private string _displayName = "Settings";
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
        #endregion // Fields

        #region FieldEncapsulation
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
                    OnPropertyChanged("CopyKey");
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
                    OnPropertyChanged("EditKey");
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
                    OnPropertyChanged("SaveKey");
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
                    OnPropertyChanged("ClearKey");
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
                    OnPropertyChanged("SelectAll");
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
                    OnPropertyChanged("BorderKey");
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
                    OnPropertyChanged("TipsKey");
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
                    OnPropertyChanged("SwitchScreenKey");
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
                    OnPropertyChanged("SettingsWin");
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
                    OnPropertyChanged("QuitKey");
                }
            }
        }
        #endregion // FieldEncapsulation

        #region Constructors
        public SettingsViewModel()
        {
            _settingsInst = Properties.Settings.Default;
        }
        #endregion // Constructors

    }
}
