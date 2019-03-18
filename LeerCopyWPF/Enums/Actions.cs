using System;
using System.Collections.Generic;
using System.Text;

namespace LeerCopyWPF.Enums
{
    public class Actions
    {
        /// <summary>
        /// All actions able to be performed during selection
        /// </summary>
        public enum ActionEnum
        {
            Invalid,
            Copy,
            Edit,
            Save,
            SelectAll,
            Clear,
            New,
            Settings,
            Tips,
            Quit
        }
    }
}
