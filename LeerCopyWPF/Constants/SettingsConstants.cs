using System;
using System.Collections.Generic;
using System.Text;

namespace LeerCopyWPF.Constants
{
    public static class SettingsConstants
    {
        /// <summary>
        /// 'Copy selection' action key binding setting
        /// </summary>
        public const string CopySettingName = "CopyKey";

        /// <summary>
        /// 'Edit selection' action key binding setting
        /// </summary>
        public const string EditSettingName = "EditKey";

        /// <summary>
        /// 'Save selection' action key binding setting
        /// </summary>
        public const string SaveSettingName = "SaveKey";

        /// <summary>
        /// 'Clear selection' action key binding setting
        /// </summary>
        public const string ClearSettingName = "ClearKey";

        /// <summary>
        /// 'Select all' action key binding setting
        /// </summary>
        public const string SelectAllSettingName = "SelectAll";

        /// <summary>
        /// 'Toggle border' action key binding setting
        /// </summary>
        public const string BorderSettingName = "BorderKey";

        /// <summary>
        /// 'Toggle tips' action key binding setting
        /// </summary>
        public const string TipsSettingName = "TipsKey";
        
        /// <summary>
        /// 'Open settings' action key binding setting
        /// </summary>
        public const string SettingsSettingName = "SettingsWin";

        /// <summary>
        /// 'Quit selection' action key binding setting
        /// </summary>
        public const string QuitSettingName = "QuitKey";

        /// <summary>
        /// Property name for default file extension setting
        /// </summary>
        public const string ConstFileExtPropName = "DefaultSaveExt";

        /// <summary>
        /// Property name for default file name setting
        /// </summary>
        public const string ConstFileNamePropName = "DefaultFileName";

        /// <summary>
        /// Property name for border visibility setting
        /// </summary>
        public const string ConstSelectBorderVisPropName = "BorderVisibility";

        /// <summary>
        /// Property name for tips visibility setting
        /// </summary>
        public const string ConstTipsVisPropName = "TipsVisibility";

        /// <summary>
        /// Property name for window opacity setting
        /// </summary>
        public const string ConstSelectWinOpacityPropName = "SelectionWinOpacity";

        /// <summary>
        /// Max opacity for the selection window's background
        /// </summary>
        public const double ConstSelectOpacityMax = 0.3;
    }
}
