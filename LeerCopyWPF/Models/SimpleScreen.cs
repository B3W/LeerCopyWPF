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

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LeerCopyWPF.Models
{
    /// <summary>
    /// 'Wrapper' class for System.Windows.Forms.Screen to hold relevant
    /// screen data for screen capturing
    /// </summary>
    public class SimpleScreen : IEquatable<SimpleScreen>, IComparable<SimpleScreen>
    {
        /// <summary>
        /// The number of bits of memory, associated with one pixel of data
        /// </summary>
        public int BitsPerPixel { get; set; }
        /// <summary>
        /// Bounds of the Bitmap relative to screen captured on
        /// </summary>
        public Rect Bounds { get; set; }
        /// <summary>
        /// Name of the device associated with screen
        /// </summary>
        public string DeviceName { get; set; }


        public SimpleScreen()
        {
            BitsPerPixel = -1;
            Bounds = new Rect();
            DeviceName = "<none>";
        }


        public SimpleScreen(int bpp, Rect bounds, string name)
        {
            BitsPerPixel = bpp;
            Bounds = bounds;
            DeviceName = name;
        }


        /// <summary>
        /// Checks equality of this ExtBitmapSource object to 'obj' as an ExtBitmapSource
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal, false otherwise</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (!(obj is SimpleScreen ss))
            {
                return false;
            }

            return this.Equals(obj as SimpleScreen);
        } // Equals


        /// <summary>
        /// Checks equality of this ExtBitmapSource object with other
        /// </summary>
        /// <param name="other"></param>
        /// <returns>True if equal, false otherwise</returns>
        public bool Equals(SimpleScreen other)
        {
            if (other == null)
            {
                return false;
            }

            return (Bounds.Equals(other.Bounds)) && (DeviceName.Equals(other.DeviceName));
        } // Equals


        /// <summary>
        /// Calculates object's hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        } // GetHashCode


        /// <summary>
        /// Compares this SimpleScreen object with other. Top, left is least while bottom, right is greatest value.
        /// Vertical positioning has greater priority than horizontal positioning.
        /// -1 -1 -1
        /// -1  x  1
        ///  1  1  1
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        /// Less than 0 if this object precedes other in sort order,
        /// 0 if this object occurs in the same position in the sort order as other,
        /// or greater than 0 if this object follows other in sort order
        /// </returns>
        public int CompareTo(SimpleScreen other)
        {
            Rect oBounds = other.Bounds;

            if (Bounds.Equals(oBounds))
            {
                return 0;
            }
            else if (Bounds.Right <= oBounds.Left) // 'This' left of 'other'
            {
                if (Bounds.Bottom <= oBounds.Top) // 'This' above 'other'
                {
                    return -1;
                }
                else if (Bounds.Top >= oBounds.Bottom) // 'This' below 'other'
                {
                    return 1;
                }
                else // 'This' in line with 'other'
                {
                    return -1;
                }
            }
            else if (Bounds.Left >= oBounds.Right) // 'This' right of 'other'
            {
                if (Bounds.Bottom <= oBounds.Top) // 'This' above 'other'
                {
                    return -1;
                }
                else if (Bounds.Top >= oBounds.Bottom) // 'This' below 'other'
                {
                    return 1;
                }
                else // 'This' in line with 'other'
                {
                    return 1;
                }
            }
            else // 'This' overlapping with 'other' horizontally
            {
                if (Bounds.Bottom <= oBounds.Top) // 'This' above 'other'
                {
                    return -1;
                }
                else if (Bounds.Top >= oBounds.Bottom) // 'This' below 'other'
                {
                    return 1;
                }
                else // 'This' overlapping with 'other' vertically
                {
                    if (Bounds.Top < oBounds.Top || Bounds.Left < oBounds.Left) // 'This' left/up, left/up/up, right of 'other'
                    {
                        return -1;
                    }
                    else // 'This' down/down, right/right of 'other'
                    {
                        return 1;
                    }
                }
            }
        } // CompareTo
    }
}
