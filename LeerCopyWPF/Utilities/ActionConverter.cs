﻿/*
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

using LeerCopyWPF.Enums;

namespace LeerCopyWPF.Utilities
{
    public static class ActionConverter
    {
        /// <summary>
        /// Converts a string representing a key value into an action enum
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static KeyActions.KeyUp KeyUpStrToEnum(string Key)
        {
            KeyActions.KeyUp retEnum;

            if (Key.Equals("CopyKey"))
            {
                retEnum = KeyActions.KeyUp.Copy;
            }
            else if (Key.Equals("EditKey"))
            {
                retEnum = KeyActions.KeyUp.Edit;
            }
            else if (Key.Equals("SaveKey"))
            {
                retEnum = KeyActions.KeyUp.Save;
            }
            else if (Key.Equals("SelectAll"))
            {
                retEnum = KeyActions.KeyUp.SelectAll;
            }
            else if (Key.Equals("ClearKey"))
            {
                retEnum = KeyActions.KeyUp.Clear;
            }
            else if (Key.Equals("BorderKey"))
            {
                retEnum = KeyActions.KeyUp.Border;
            }
            else if (Key.Equals("TipsKey"))
            {
                retEnum = KeyActions.KeyUp.Tips;
            }
            else if (Key.Equals("QuitKey"))
            {
                retEnum = KeyActions.KeyUp.Quit;
            }
            else if (Key.Equals("SettingsWin"))
            {
                retEnum = KeyActions.KeyUp.Settings;
            }
            else if (Key.Equals("SwitchScreenKey"))
            {
                retEnum = KeyActions.KeyUp.Switch;
            }
            else
            {
                retEnum = KeyActions.KeyUp.Invalid;
            }

            return retEnum;
        } // KeyStrToEnum


        /// <summary>
        /// Converts action enum to its key string equivalent
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string EnumToKeyUpStr(KeyActions.KeyUp action)
        {
            string retStr;

            switch (action)
            {
                case KeyActions.KeyUp.Invalid:
                    retStr = null;
                    break;
                case KeyActions.KeyUp.Copy:
                    retStr = "CopyKey";
                    break;
                case KeyActions.KeyUp.Edit:
                    retStr = "EditKey";
                    break;
                case KeyActions.KeyUp.Save:
                    retStr = "SaveKey";
                    break;
                case KeyActions.KeyUp.SelectAll:
                    retStr = "SelectAll";
                    break;
                case KeyActions.KeyUp.Clear:
                    retStr = "ClearKey";
                    break;
                case KeyActions.KeyUp.Border:
                    retStr = "BorderKey";
                    break;
                case KeyActions.KeyUp.Tips:
                    retStr = "TipsKey";
                    break;
                case KeyActions.KeyUp.Switch:
                    retStr = "SwitchScreenKey";
                    break;
                case KeyActions.KeyUp.Settings:
                    retStr = "SettingWin";
                    break;
                case KeyActions.KeyUp.Quit:
                    retStr = "QuitKey";
                    break;
                default:
                    retStr = null;
                    break;
            }
            return retStr;
        } // EnumToKeyStr
    }
}
