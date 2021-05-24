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
