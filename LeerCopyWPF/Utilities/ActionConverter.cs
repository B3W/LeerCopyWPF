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
        public static Actions.ActionEnum KeyStrToEnum(string Key)
        {
            Actions.ActionEnum retEnum;

            if (Key.Equals("CopyKey"))
            {
                retEnum = Actions.ActionEnum.Copy;
            }
            else if (Key.Equals("EditKey"))
            {
                retEnum = Actions.ActionEnum.Edit;
            }
            else if (Key.Equals("SaveKey"))
            {
                retEnum = Actions.ActionEnum.Save;
            }
            else if (Key.Equals("SelectAll"))
            {
                retEnum = Actions.ActionEnum.SelectAll;
            }
            else if (Key.Equals("ClearKey"))
            {
                retEnum = Actions.ActionEnum.Clear;
            }
            else if (Key.Equals("Tips"))
            {
                retEnum = Actions.ActionEnum.Tips;
            }
            else if (Key.Equals("QuitKey"))
            {
                retEnum = Actions.ActionEnum.Quit;
            }
            else if (Key.Equals("NewLeer"))
            {
                retEnum = Actions.ActionEnum.New;
            }
            else if (Key.Equals("SettingsWin"))
            {
                retEnum = Actions.ActionEnum.Settings;
            }
            else
            {
                retEnum = Actions.ActionEnum.Invalid;
            }

            return retEnum;
        } // KeyStrToEnum


        /// <summary>
        /// Converts action enum to its key string equivalent
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string EnumToKeyStr(Actions.ActionEnum action)
        {
            string retStr;

            switch (action)
            {
                case Actions.ActionEnum.Invalid:
                    retStr = null;
                    break;
                case Actions.ActionEnum.Copy:
                    retStr = "CopyKey";
                    break;
                case Actions.ActionEnum.Edit:
                    retStr = "EditKey";
                    break;
                case Actions.ActionEnum.Save:
                    retStr = "SaveKey";
                    break;
                case Actions.ActionEnum.SelectAll:
                    retStr = "SelectAll";
                    break;
                case Actions.ActionEnum.Clear:
                    retStr = "ClearKey";
                    break;
                case Actions.ActionEnum.New:
                    retStr = "NewLeer";
                    break;
                case Actions.ActionEnum.Settings:
                    retStr = "SettingWin";
                    break;
                case Actions.ActionEnum.Tips:
                    retStr = "Tips";
                    break;
                case Actions.ActionEnum.Quit:
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
