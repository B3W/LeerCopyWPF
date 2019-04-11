using System;
using System.Collections.Generic;
using System.Text;

namespace LeerCopyWPF.Enums
{
    public class KeyActions
    {
        /// <summary>
        /// All key up actions able to be performed during selection
        /// </summary>
        public enum KeyUp
        {
            Invalid,
            Copy,
            Edit,
            Save,
            SelectAll,
            Clear,
            Border,
            Tips,
            Switch,
            Settings,
            Quit
        }

        /// <summary>
        /// All key down actions able to be performed during selection
        /// </summary>
        public enum KeyDown
        {
            Invalid,
            Up,
            Down,
            Left,
            Right
        }
    }
}
