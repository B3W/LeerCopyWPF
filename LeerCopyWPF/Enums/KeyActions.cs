/*
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

namespace LeerCopyWPF.Enums
{
    /// <summary>
    /// All key up actions able to be performed during selection
    /// </summary>
    public enum KeyUpAction
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
    /// All possible directions to resize selection
    /// </summary>
    public enum ResizeDirection
    {
        Invalid,
        Up,
        Down,
        Left,
        Right
    }
}
